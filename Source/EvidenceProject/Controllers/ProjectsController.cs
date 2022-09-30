using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using EvidenceProject.Helpers;


namespace EvidenceProject.Controllers;
public class ProjectController : Controller
{
    private readonly ProjectContext _context;

    public ProjectController(ProjectContext context)
    {
        _context = context;
    }

    [HttpGet("project")]
    public ActionResult Index()
    {
        if(!UniversalHelper.getLoggedUser(HttpContext, out var userID)) return Redirect("/");
        return Redirect("project/create");
    }

    /// <summary>
    /// Vytvoření/přidání projektu
    /// </summary>        
    [HttpPost("project/create")]
    public ActionResult Create([FromForm] ProjectCreateData projectData)
    {
        Project project = new()
        {
            name = projectData.projectName
        };
        _context?.projects?.Add(project);
        _context?.SaveChanges();
        return Redirect("Index");
    }

    [HttpGet("project/create")]
    public ActionResult Create()
    {
        var dialCodes = _context?.dialCodes?.ToList();
        return View(dialCodes);
    }

    /// <summary>
    /// Odstranění projektu
    /// </summary>
    [HttpPost("project/{id}")]
    public ActionResult Delete(int id)
    {
        if (!UniversalHelper.getProject(_context, id, out var project)) return Json("Takový projekt neexistuje");
        _context?.projects?.Remove(project);
        return Json("ok");
    }
    
    /// <summary>
    /// Stránka s projektem 
    /// </summary>
    [HttpGet("projectinfo/{id}")]
    public ActionResult ProjectInfo(int id)
    {
        if (!UniversalHelper.getProject(_context, id, out var project)) return View();
        return View(project);
    }

    /// <summary>
    /// Vyhledávání
    /// </summary>
    [HttpPost("search")]
    public ActionResult Search(string searchQuery)
    {
        if (searchQuery == string.Empty) return Ok();
        var projects = _context?.projects?.Where(project => project.name.Contains(searchQuery));
        if (projects == null) return Json("Nic nenalezeno");
        // JSON OR VIEW ?
        return View(projects);
    }
}
