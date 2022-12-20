using EvidenceProject.Controllers.ActionData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/");
        return Redirect("project/create");
    }

    /// <summary>
    ///     Vytvoření/přidání projektu
    /// </summary>
    [HttpPost("project/create")]
    public ActionResult Create([FromForm] ProjectCreateData projectData, bool test = false)
    {
        int? userID = 0;

        if (!test) if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Json("ERR");

        var GETProject = GetProjectCreateData();
        if (!UniversalHelper.CheckAllParams(projectData)) return View(GETProject.SetError("Něco nebylo vyplněno"));


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

        List<User> users = new();

        var contextUsers = _context.users;
        if(!projectData.assignees.IsNull())
        {
            foreach (var item in projectData.assignees)
            {
                var user = contextUsers?.FirstOrDefault(u => u.fullName == item);
                if (user.IsNull()) continue;
                users.Add(user);
            }
        }

        List<Achievement> achivements = new();

        Project project = new()
        {
            name = projectData.projectName,
            projectTechnology = tech,
            projectType = _context.dialCodes.FirstOrDefault(x => x.name == projectData.typy),
            assignees = users,
            github = projectData.github,
            slack = projectData.slack,
            projectAchievements = null,
            files = files,
            projectState = _context.dialCodes.FirstOrDefault(x => x.name == projectData.stavit),
            projectDescription = projectData.description,
            projectManager = _context.globalUsers.FirstOrDefault(x => x.fullName == projectData.projectManager)
        };

        if (!projectData.achievements.IsNull())
        {
            var splittedAchievements = projectData?.achievements?.Split(";");
            foreach (var item in splittedAchievements)
            {
                achivements.Add(new Achievement()
                {
                    name = item,
                    project = project,
                });
            }

        }
        project.projectAchievements = achivements;

        _logger.LogInformation("User with the id <{}> created a project called \"{}\"", userID, projectData.projectName);
        _context?.projects?.Add(project);
        _context?.SaveChanges();
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
        return Redirect("Index");
    }

    /// <summary>
    ///     Create (get)
    /// </summary>
    [HttpGet("project/create")]
    public ActionResult Create()
    {
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/");
        var data = GetProjectCreateData();
        return View(data);

    }

    /// <summary>
    ///     Odstranění projektu
    /// </summary>
    [HttpPost("project/{id}")]
    public JsonResult Delete(int id)
    {
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context, out var userID)) return Json("Nejsi admin/přihlášen");
        var project = UniversalHelper.GetProject(_context, id);
        if (project == null) return Json("Takový projekt neexistuje");
        _logger.LogInformation("User with the id <{}> deleted a project", userID);

        _context?.projects?.Remove(project);
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
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
        if(project.projectManager?.id != userID && _context.globalUsers?.FirstOrDefault(u=>u.id == userID)?.globalAdmin.Value != true) return Redirect("/user/profile/");

        GETProjectCreate data = new();
        var GETProjectData = GetProjectCreateData();
        data.CurrentProject = project;
        data.Users = GETProjectData.Users;
        data.DialInfos = GETProjectData.DialInfos;
        data.DialCodes = GETProjectData.DialCodes;
       
        return View(data);
    }

    [HttpPost("project/edit/{id}")]
    public ActionResult Edit(int id, [FromForm] ProjectEditData projectData, bool test = false)
    {
        int? userID = 0;

        if (!test) if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context, out userID)) return Json("ERR");

        Project? project = UniversalHelper.GetProject(_context, id);
        var GETProject = GetProjectCreateData();
        GETProject.CurrentProject = project;
        if (!UniversalHelper.CheckAllParams(projectData)) return View(GETProject.SetError("Něco nebylo vyplněno"));


        if (project == null) return View(GETProject.SetError("Takový projekt neexistuje"));


        if(projectData.oldFile.IsNull() && projectData.photos.IsNull()) return View(GETProject.SetError("Nebyl nahrán žádný soubor"));
        if(projectData.oldTech.IsNull() && projectData.tech.IsNull()) return View(GETProject.SetError("Pro editaci je potřeba min. 1 kategorie"));

        if (projectData.oldFile != null) foreach (var item in project?.files?.ToList()) if (!projectData.oldFile.Contains(item.generatedFileName)) project.files.Remove(item);
        

        if(projectData?.photos != null)
        {
            foreach (var file in projectData?.photos)
            {
                var extension = Path.GetExtension(file.FileName);
                var dbFile = new DbFile();
                dbFile.WriteFile(file);
                project?.files?.Add(dbFile);
            }
        }

        if (projectData.oldTech != null) foreach (var item in project?.projectTechnology?.ToList()) if (projectData.oldFile.Contains(item.name)) project.projectTechnology.Remove(item);

        if(projectData.tech != null)
        {
            foreach (var item in projectData.tech)
            {
                var dialCode = _context?.dialCodes?.FirstOrDefault(x => x.name == item);
                if (dialCode == null) continue;
                project?.projectTechnology?.Add(dialCode);
            }
        }

        HashSet<User> users = new();
        var contextUsers = _context.users;
        GetAssignees(users, projectData.assignees, contextUsers);
        GetAssignees(users, projectData.assignees, contextUsers);
        List<Achievement> achivements = new();

        var splittedAchievements = projectData?.achievements?.Split(";");

        project.name = projectData.projectName;
        project.projectType = _context.dialCodes.FirstOrDefault(x => x.name == projectData.typy);
        project.assignees = users.ToList();
        project.github = projectData.github;
        project.slack = projectData.slack;
        project.projectState = _context.dialCodes.FirstOrDefault(x => x.name == projectData.stavit);
        project.projectDescription = projectData.description;
        project.projectManager = _context.globalUsers.FirstOrDefault(x => x.fullName == projectData.projectManager);

        if (splittedAchievements != null)
            foreach (var item in splittedAchievements)
            {
                achivements.Add(new Achievement()
                {
                    name = item,
                    project = project,
                });
            }
        project.projectAchievements = achivements;


        _logger.LogInformation("User with the id <{}> edited a project called \"{}\"", userID, projectData.projectName);
        _context?.projects?.Update(project);
        _context?.SaveChanges();
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
        return Redirect("Index");
    }

    [HttpPost("/project/apply/{id}")]
    public ActionResult Apply(int id,[FromForm] ProjectApplyData data)
    {
        if (!UniversalHelper.CheckAllParams(data)) return Redirect($"/project/{id}");

        var project = UniversalHelper.GetProject(_context, id);
        if(project.IsNull()) return Redirect($"/project/{id}");

        if (project.applicants == null) project.applicants = new List<User>();
        
        var user = _context.users?.FirstOrDefault(u => u.fullName == $"{data.firstname} {data.lastname}" && u.contactDetails == data.contact && u.schoolYear == u.schoolYear);
        if(user == null)
        {
            user = new User()
            {
                contactDetails = data.contact,
                fullName = $"{data.firstname} {data?.lastname}",
                schoolYear = byte.Parse(data.schoolYear),
                studyField = data.studyField
            };
            _context.users?.Add(user);
        }

        if(!project.applicants.Contains(user)) project?.applicants?.Add(user);

        _context.projects?.Update(project);
        _context.SaveChanges();
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
        return Redirect($"/project/{id}");
    }


    [HttpPost("/project/addUser/{id}")]
    public ActionResult AddUser(int projectId, int id)
    {
        DoSometingWithUser(projectId, id);
        return Redirect("/user/profile");
    }

    [HttpPost("/project/deleteUser/{id}")]
    public ActionResult DeleteUserAction(int projectId, int id)
    {
        DoSometingWithUser(projectId, id, false);
        return Redirect("/user/profile");
    }

    /// <summary>
    /// TODO UKLIDIT!
    /// </summary>
    private void DoSometingWithUser(int projectId, int id, bool add = true)
    {
        var project = UniversalHelper.GetProject(_context, projectId);
        if (project == null) return;

        var applicant = project.applicants?.FirstOrDefault(a => a.id == id);
        if (applicant == null) return;

        var user = _context.users.FirstOrDefault(u => u.id == id);
        if (add)
        {
            if(!user.IsNull())
            {
                user.Projects = user.Projects ?? new List<Project>();
                user.Projects.Add(project);
                project.assignees?.Add(applicant);
            }
        }
        else _context.users.Remove(user);
        project.applicants?.Remove(applicant);
        _context.SaveChanges();
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
    }


    private GETProjectCreate GetProjectCreateData()
    {
        GETProjectCreate GETProject = new();

        GETProject.DialInfos = UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos");
        // toto nemůžeme brát z cache, je zde include!
        GETProject.DialCodes = _context.dialCodes.Include(x => x.dialInfo).ToList();
        GETProject.Users = _context.users.ToList();
        return GETProject;
    }

    private void GetAssignees(HashSet<User> users, string[] data, DbSet<User> contextUsers)
    {
        if (data.IsNull()) return;

        foreach (var assignee in data)
        {
            var user = contextUsers?.FirstOrDefault(u => u.fullName == assignee);
            if (user.IsNull()) continue;
            users.Add(user);
        }
    }
}