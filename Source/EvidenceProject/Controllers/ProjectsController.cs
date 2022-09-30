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
        // Počkám na dokumentaci z db sekce
        Project project = new()
        {
            name = projectData.ProjectName
        };
        _context.projects.Add(project);
        _context.SaveChanges();
        return Redirect("Index");
    }

    [HttpGet("project/create")]
    public ActionResult Create()
    {
        var dialCodes = _context.dialCodes.ToList();
        return View(dialCodes);
    }

    [HttpPost("project/{id}")]
    public ActionResult Delete(int id)
    {
        return Json("ok");
    }

    /// <param name="id">Id projektu</param>
    [HttpGet("projectinfo/{id}")]
    public ActionResult ProjectInfo(int id)
    {
        return View();
    }
}
