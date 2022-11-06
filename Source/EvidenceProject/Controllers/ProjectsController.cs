namespace EvidenceProject.Controllers;
public class ProjectController : Controller
{
    private readonly ProjectContext _context;

    public ProjectController(ProjectContext context) => _context = context;

    /// <summary>
    /// projekt (get)
    /// </summary>
    [HttpGet("project")]
    public ActionResult Index()
    {
        if(!UniversalHelper.getLoggedUser(HttpContext, out var userID) && userID != "1") return Redirect("/");
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

    /// <summary>
    /// Create (get)
    /// </summary>
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
    public JsonResult Delete(int id)
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
        List<Project> projects  = _context?.projects?.ToList().Where(project => project.name.Contains(searchQuery)).ToList();
        if (projects == null) return Json("Nic nenalezeno");
        // Budeme posílat JSON, ať si to JS užijí :D
        return Json(projects);
    }
}
