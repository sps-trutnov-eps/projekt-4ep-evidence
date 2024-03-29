﻿using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace EvidenceProject.Helpers;

public static class UniversalHelper
{
    /// <summary>
    ///     Jméno cookie
    /// </summary>
    public static string LoggedInKey => "loggedin";

    /// <summary>
    ///     Jméno cookie pro uživatele
    /// </summary>
    public static string LoggedNameKey => "loggedinName";

    /// <summary>
    ///     Jméno cookie
    /// </summary>
    public static string IsAdmin => "_IsAdmin_";

    /// <summary>
    ///     Json hláška při chybě
    /// </summary>
    public static string SomethingWentWrongMessage => "Něco se pokazilo";

    /// <summary>
    ///     Klíč k listu zacachovaných dialInfos
    /// </summary>
    public static string DialInfoCacheKey => "dialInfos";

    /// <summary>
    ///     Klíč k listu zacachovaných dialCodes
    /// </summary>
    public static string DialCodeCacheKey => "dialCodes";

    /// <summary>
    ///     Klíč k listu zacachovaných globalUsers
    /// </summary>
    public static string GlobalUsersCacheKey => "globalUsers";


    /// <summary>
    /// Chybová hláška, která se nastaví v HttpContextu
    /// Fuj ViewData,..
    /// </summary>
    public static string RedirectError => "RedirectError";

    /// <summary>
    ///     Zjistíme, zda je přihlášen uživatel
    /// </summary>
    public static bool GetLoggedUser(HttpContext context, out int? userID)
    {
        userID = context.Session.GetInt32(LoggedInKey);
        return userID != null;
    }

    /// <summary>
    ///     Zjistí, zda je přihlášený uživatel Admin.
    /// </summary>
    /// <param name="context">httpContext</param>
    /// <param name="db">Database</param>
    /// <returns></returns>
    public static bool AuthentifyAdmin(HttpContext context, ProjectContext db)
    {
        var user = GetUser(db, context);

        if (user == null) return false;
        return user.globalAdmin == true;
    }

    /// <summary>
    ///     Zjistí, zda je přihlášený uživatel Admin a vrátí jeho ID.
    /// </summary>
    /// <param name="context">httpContext</param>
    /// <param name="db">Database</param>
    /// <returns></returns>
    public static bool AuthentifyAdmin(HttpContext context, ProjectContext db, out int? ID)
    {
        var user = GetUser(db, context);

        ID = 0;
        if (user == null) return false;

        ID = user.id;
        return user.globalAdmin == true;
    }

    private static AuthUser? GetUser(ProjectContext db, HttpContext httpContext)
    {
        var userID = httpContext.Session.GetInt32(LoggedInKey);
        var user = db.globalUsers?.FirstOrDefault(u => u.id == userID);
        return user;
    }

    /// <summary>
    /// Získání všech projektů
    /// </summary>    
    public static List<Project>? GetProjectsWithIncludes(ProjectContext context)
    {
        if (context.projects?.ToList().Count == 0 || context.projects?.ToList()== null) return null;
        var projects = context.projects?
            .Include(x => x.projectTechnology)
            .Include(x => x.projectType)
            .Include(x => x.projectState)
            .Include(x => x.files)
            .Include(x => x.projectAchievements)
            .Include(x => x.projectManager)
            .Include(x => x.applicants)
            .Include(x => x.assignees)
            .ToList();

        return projects;
    }

    /// <summary>
    /// Získání dle id
    /// </summary>    
    public static Project? GetProject(ProjectContext context, int id) => GetProjectsWithIncludes(context)?.FirstOrDefault(x => x.id == id);
    
    /// <summary>
    /// Pokud něco bude prázdné v objektu, vrátí null
    /// </summary>
    /// <param name="obj">Object</param>
    /// <param name="toDontCheck">Které params nemáme kontrolovat</param>

    public static bool CheckAllParams(object obj)
    {
        var type = obj.GetType();
        var props = type.GetProperties(BindingFlags.Instance | System.Reflection.BindingFlags.Public)
        .Where(w => w.CanRead && w.CanWrite)
        .Where(w => w.GetGetMethod(true).IsPublic)
        .Where(w => w.GetSetMethod(true).IsPublic);
        foreach (var prop in props)
        {
            var attrs = prop.GetCustomAttributes().ToList();
            if (attrs.Any(x => x.GetType().Name == "NotRequired")) continue;
            var propValue = (type.GetProperty(prop.Name).GetValue(obj, null) ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(propValue)) return false;
        }
        return true;
    }

    /// <summary>
    /// Získáme data z cache
    /// </summary>
    public static List<T>? GetData<T>(ProjectContext context, IMemoryCache cache, string cacheKey, string propertyName, bool project = false)
    {
        var data = (List<T>)cache.Get(cacheKey);
        if (data != null) return data;

        IEnumerable<T> dbData = project? (IEnumerable<T>)GetProjectsWithIncludes(context) : (IEnumerable<T>)context.GetType().GetProperty(propertyName).GetValue(context, null);
        if(dbData == null)
        {
            cache.Set(cacheKey, new List<T>());
            return null;
        }
        var listData = dbData.ToList();
        cache.Set(cacheKey, listData);
        return listData;
    }

    /// <summary>
    /// Získání stringu achievementů z listu
    /// </summary>
    public static string? GetAchievements(List<Achievement>? achievements, bool withParser = false)
    {
        if (achievements == null) return null;
        string achievementString = "";
        string parser = withParser ? ";" : " ";
        foreach (var item in achievements) achievementString += $"{item.name}{parser}";
        return achievementString;
    }


    public static string GetDataFromWWWRoot(string fileName, bool css = true)
    {
        // Pro případnou editaci js ze stránky
        var toCss = css == true ? "css" : "js";
        string path = $"./wwwroot/{toCss}/{fileName}";
        string text = string.Empty;
        try
        {
            text = File.ReadAllText(path);
        }
        catch
        {
            text = $"Nebyl nalezen {fileName} v cestě {path}";
        }
        return text;
    }
    public static bool IsNull(this object? obj) => obj == null;

    public static void UpdateProjectsInCache(IMemoryCache cache, ProjectContext context)
    {
        var projects = GetProjectsWithIncludes(context);
        cache.Set("AllProjects", projects);
    }

    /// <summary>
    /// Získání chybové hlášky z httcontextu
    /// </summary>
    public static bool TryGetErrorMessage(HttpContext httpContext, out string? message)
    {
        message = httpContext.Session.GetString(UniversalHelper.RedirectError);
        if (message.IsNull()) return false;
        httpContext.Session.Remove(UniversalHelper.RedirectError);
        return true;
    }

    /// <summary>
    /// Maximální délka popisu projetku na administraci/profilu
    /// </summary>
    public static int MaxDescSize => 30;

    /// <summary>
    /// Získáme zkrácený popis
    /// Určeno pro výpis, profil a administraci
    /// </summary>
    public static string GetTrimmedDescription(Project project) => MaxDescSize >= project.projectDescription.Length ? project.projectDescription : $"{project.projectDescription.Substring(0, MaxDescSize)}...";
}
