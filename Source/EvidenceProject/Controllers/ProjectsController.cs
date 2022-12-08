using EvidenceProject.Controllers.ActionData;
using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Reflection;

namespace EvidenceProject.Controllers;

public class ProjectController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly ProjectContext _context;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ProjectContext context, IMemoryCache cache, ILogger<ProjectController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    ///     projekt (get)
    /// </summary>
    [HttpGet("project")]
    public ActionResult Index()
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != 1) return Redirect("/");
        return Redirect("project/create");
    }

    /// <summary>
    ///     Vytvoření/přidání projektu
    /// </summary>
    [HttpPost("project/create")]
    public ActionResult Create([FromForm] ProjectCreateData projectData, bool test = false)
    {
        int? userID = 0;

        if (!test) if (!UniversalHelper.GetLoggedUser(HttpContext, out userID) && userID != 1) return Json("ERR");

        var GETProject = GetProjectCreateData();
        if (!UniversalHelper.CheckAllParams(projectData, UniversalHelper.NoCheckParamsProject)) return View(GETProject);


        List<DbFile> files = new();
        foreach (var file in projectData?.photos)
        {
            var dbFile = new DbFile();
            dbFile.WriteFile(file);
            files.Add(dbFile);
        }
        List<DialCode> tech = new();

        foreach (var item in projectData.tech)
        {
            var dialCode = _context?.dialCodes?.FirstOrDefault(x => x.name == item);
            if (dialCode == null) continue;
            tech.Add(dialCode);
        }

        List<Achievement> achivements = new();

        var splittedAchievements = projectData?.achievements?.Split(";");

        // Správný způsob načítání achivementů
        //foreach (string achiv in projectData.achievements ?? new List<string>() { string.Empty })
        //    achivements.Add(new Achievement() { name = achiv });

        Project project = new()
        {
            name = projectData.projectName,
            projectTechnology = tech,
            projectType = _context.dialCodes.FirstOrDefault(x => x.name == projectData.typy),
            assignees = null,
            github = projectData.github,
            slack = projectData.slack,
            projectAchievements = null,
            files = files,
            projectState = _context.dialCodes.FirstOrDefault(x => x.name == projectData.stavit),
            projectDescription = projectData.description,
            projectManager = _context.globalUsers.FirstOrDefault(x => x.fullName == projectData.projectManager)
        };

        if(splittedAchievements != null)
            foreach (var item in splittedAchievements)
            {
                achivements.Add(new Achievement()
                {
                    name = item,
                    project = project,
                });
            }
        project.projectAchievements = achivements;

        _logger.LogInformation("User with the id <{}> created a project called \"{}\"", userID, projectData.projectName);
        _context?.projects?.Add(project);
        _context?.SaveChanges();
        UpdateProjectsInCache();
        return Redirect("Index");
    }

    /// <summary>
    ///     Create (get)
    /// </summary>
    [HttpGet("project/create")]
    public ActionResult Create()
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != 1) return Redirect("/");
        var data = GetProjectCreateData();
        return View(data);

    }

    /// <summary>
    ///     Odstranění projektu
    /// </summary>
    [HttpPost("project/{id}")]
    public JsonResult Delete(int id)
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != 1) return Json("Nejsi admin/přihlášen");
        var project = UniversalHelper.GetProject(_context, id);
        if (project == null) return Json("Takový projekt neexistuje");
        _logger.LogInformation("User with the id <{}> deleted a project", userID);

        _context?.projects?.Remove(project);
        UpdateProjectsInCache();
        _context?.SaveChanges();
        return Json("ok");
    }

    /// <summary>
    ///     Stránka s projektem
    /// </summary>
    [HttpGet("project/{id}")]
    public ActionResult Project(int id)
    {
        var project = UniversalHelper.GetProject(_context, id);
        if (project == null) return Redirect("/");
        return View(project);
    }

    /// <summary>
    ///     Vyhledávání
    /// </summary>
    [HttpPost("search")]
    public ActionResult Search([FromForm] SearchData data)
    {
        if (data.text == string.Empty) return Ok();
        var projects = UniversalHelper.GetProjectsWithIncludes(_context)?.Where(project => project.name.Contains(data.text)).ToList();
        if (projects == null) return Json("Nic nenalezeno");
        return Json(projects);
    }

    [HttpGet("project/edit/{id}")]
    public ActionResult Edit(int id)
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID)) 
            return Redirect("/user/profile/");
        var project = UniversalHelper.GetProject(_context, id);
        if (project == null) return Redirect("/user/profile/");
        if(project.projectManager?.id != userID && userID != 1) return Redirect("/user/profile/");

        GETProjectEdit data = new();
        var GETProjectData = GetProjectCreateData();
        data.CurrentData = project;
        data.Users = GETProjectData.Users;
        data.DialInfos = GETProjectData.DialInfos;
        data.DialCodes = GETProjectData.DialCodes;

       
        return View(data);
    }

    [HttpPost("project/edit/{id}")]
    public ActionResult Edit(int id, [FromForm] ProjectCreateData projectData, bool test = false)
    {
        int? userID = 0;

        if (!test) if (!UniversalHelper.GetLoggedUser(HttpContext, out userID) && userID != 1) return Json("ERR");

        var GETProject = GetProjectCreateData();
        if (!UniversalHelper.CheckAllParams(projectData)) return View(GETProject);

        Project? project = _context.projects?.FirstOrDefault(p => p.id == id);

        if (project == null) return Redirect("index");


        List<DbFile> files = new();
        foreach (var file in projectData?.photos)
        {
            var extension = Path.GetExtension(file.FileName);
            var dbFile = new DbFile();
            dbFile.WriteFile(file);
            files.Add(dbFile);
        }
        List<DialCode> tech = new();

        foreach (var item in projectData.tech)
        {
            var dialCode = _context?.dialCodes?.FirstOrDefault(x => x.name == item);
            if (dialCode == null) continue;
            tech.Add(dialCode);
        }

        List<Achievement> achivements = new();

        achivements.Add(new Achievement() { name = projectData.achievements });

        // Správný způsob načítání achivementů
        //foreach (string achiv in projectData.achievements ?? new List<string>() { string.Empty })
        //    achivements.Add(new Achievement() { name = achiv });

        project.name = projectData.projectName;
        project.projectTechnology = tech;
        project.projectType = _context.dialCodes.FirstOrDefault(x => x.name == projectData.typy);
        project.assignees = null;
        project.github = projectData.github;
        project.slack = projectData.slack;
        project.projectAchievements = achivements;
        project.files = files;
        project.projectState = _context.dialCodes.FirstOrDefault(x => x.name == projectData.stavit);
        project.projectDescription = projectData.description;
        project.projectManager = _context.globalUsers.FirstOrDefault(x => x.fullName == projectData.projectManager);

        _logger.LogInformation("User with the id <{}> edited a project called \"{}\"", userID, projectData.projectName);
        _context?.projects?.Update(project);
        _context?.SaveChanges();
        UpdateProjectsInCache();
        return Redirect("Index");
    }

    [HttpPost("/project/apply/{id}")]
    public ActionResult Apply(int id,[FromForm] ProjectApplyData data)
    {
        if (!UniversalHelper.CheckAllParams(data)) return Redirect($"/project/{id}");

        var project = _context.projects.FirstOrDefault(x => x.id == id);

        if (project?.applicants == null) project.applicants = new List<User>();

        project?.applicants?.Add(new Data.DataModels.User()
        {
            fullName = $"{data.firstname} {data.lastname}",
            schoolYear = byte.Parse(data.schoolYear),
            studyField = data.studyField,
            contactDetails = data.contact
        });

        _context.projects?.Update(project);
        _context.SaveChanges();
        UpdateProjectsInCache();
        return Redirect($"/project/{id}");
    }

    private void UpdateProjectsInCache()
    {
        var projects = UniversalHelper.GetProjectsWithIncludes(_context);
        _cache.Set("AllProjects", projects);
    }

    private GETProjectCreate GetProjectCreateData()
    {
        GETProjectCreate GETProject = new();

        GETProject.DialInfos = UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos");
        GETProject.DialCodes = UniversalHelper.GetData<DialCode>(_context, _cache, UniversalHelper.DialCodeCacheKey, "dialCodes");
        GETProject.Users = UniversalHelper.GetData<AuthUser>(_context, _cache, UniversalHelper.GlobalUsersCacheKey, "globalUsers");
        return GETProject;
    }

}