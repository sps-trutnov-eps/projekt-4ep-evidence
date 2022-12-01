using EvidenceProject.Controllers.ActionData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace EvidenceProject.Controllers;

public class ProjectController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly ProjectContext _context;
    private readonly ILogger<ProjectController> _logger;
    GETProjectCreate GETProject = new();

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
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != "1") return Redirect("/");
        return Redirect("project/create");
    }

    /// <summary>
    ///     Vytvoření/přidání projektu
    /// </summary>
    [HttpPost("project/create")]
    public ActionResult Create([FromForm] ProjectCreateData projectData, bool test = false)
    {
        var userID = string.Empty;
        if (!test) if (!UniversalHelper.GetLoggedUser(HttpContext, out userID) && userID != "1") return Json("ERR");

        List<DbFile> files = new();
        if (!UniversalHelper.CheckAllParams(projectData)) return View(GETProject);


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

        Project project = new()
        {
            name = projectData.projectName,
            projectTechnology = tech,
            projectType = _context.dialCodes.FirstOrDefault(x => x.name == projectData.typy),
            assignees = null,
            github = projectData.github,
            slack = projectData.slack,
            projectAchievements = new List<Achievement>(),
            files = files,
            projectState = _context.dialCodes.FirstOrDefault(x => x.name == projectData.stavit),
            projectDescription = projectData.description,
            projectManager = _context.globalUsers.FirstOrDefault(x => x.fullName == projectData.projectManager)
        };
        
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
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != "1") return Redirect("/");
        var dialinfos = _cache.Get(UniversalHelper.DialInfoCacheKey);
        if (dialinfos == null)
        {
            dialinfos = _context?.dialInfos?.ToList();
            _cache.Set(UniversalHelper.DialInfoCacheKey, dialinfos);
            GETProject.DialInfos = (List<DialInfo>)dialinfos;
        }
        else GETProject.DialInfos = (List<DialInfo>)dialinfos;

        // Todo cache method? 
        // TryGetFromCache(string key, out ListValue)
        //var value = _context?.GetType().GetProperty("projects")?.GetValue(_context, null);

        var dialcodes = _cache.Get(UniversalHelper.DialCodeCacheKey);
        if (dialcodes == null)
        {
            dialcodes = _context?.dialCodes?.ToList();
            _cache.Set(UniversalHelper.DialCodeCacheKey, dialcodes);
            GETProject.DialCodes = (List<DialCode>)dialcodes;
        }
        else GETProject.DialCodes = (List<DialCode>)dialcodes;

        // Todo projectManager
        GETProject.Users = _context?.globalUsers?.ToList();
        return View(GETProject);
    }

    /// <summary>
    ///     Odstranění projektu
    /// </summary>
    [HttpPost("project/{id}")]
    public JsonResult Delete(int id)
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != "1") return Json("Nejsi admin/přihlášen");
        if (!UniversalHelper.GetProject(_context, id, out var project)) return Json("Takový projekt neexistuje");
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
        if (!UniversalHelper.GetProject(_context, id, out var project)) return Redirect("/");
        return View(project);
    }

    /// <summary>
    ///     Vyhledávání
    /// </summary>
    [HttpPost("search")]
    public ActionResult Search([FromForm] SearchData data)
    {
        if (data.text == string.Empty) return Ok();
        var projects = _context?.projects?.ToList().Where(project => project.name.Contains(data.text)).ToList();
        if (projects == null) return Json("Nic nenalezeno");
        return Json(projects);
    }

    public void UpdateProjectsInCache()
    {
        var projects = _context.projects
            .Include(x => x.projectTechnology)
            .Include(x => x.projectType)
            .ToList();
        _cache.Set("AllProjects", projects);
    }


    [HttpPost("/project/apply")]
    public ActionResult Apply([FromForm] UserApplyData data) 
    {
        if (!UniversalHelper.CheckAllParams(data)) return View();
        var project = _context.projects.FirstOrDefault(x => x.id == int.Parse(data.ProjectId));
        // todo
        return Json("OK");
    }
}