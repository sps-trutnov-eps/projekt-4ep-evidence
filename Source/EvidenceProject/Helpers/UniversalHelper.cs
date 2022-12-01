using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace EvidenceProject.Helpers;

public class UniversalHelper
{
    /// <summary>
    ///     Jméno cookie
    /// </summary>
    public static string LoggedInKey => "loggedin";

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
    ///     Zjistíme, zda je přihlášen uživatel
    /// </summary>
    public static bool GetLoggedUser(HttpContext context, out string? userID)
    {
        userID = context.Session.GetString(LoggedInKey);
        return userID != null;
    }

    /// <summary>
    /// Získání všech projektů
    /// </summary>    
    public static List<Project>? GetProjectsWithIncludes(ProjectContext context)
    {
        var projects = context.projects
            .Include(x => x.projectTechnology)
            .Include(x => x.projectType)
            .Include(x => x.files)
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

    public static bool CheckAllParams(object obj)
    {
        var type = obj.GetType();
        var props = type.GetProperties(BindingFlags.Instance|System.Reflection.BindingFlags.Public)
        .Where(w => w.CanRead && w.CanWrite)
        .Where(w => w.GetGetMethod(true).IsPublic)
        .Where(w => w.GetSetMethod(true).IsPublic);
        foreach (var prop in props)
        {
            var propValue = (type.GetProperty(prop.Name).GetValue(obj, null) ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(propValue)) return false;
        }
        return true;
    }
}