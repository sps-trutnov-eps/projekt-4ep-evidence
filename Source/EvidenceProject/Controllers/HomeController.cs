using EvidenceProject.Data;

namespace EvidenceProject.Controllers;
public class HomeController : Controller
{
    private readonly ProjectContext _context;

    public HomeController(ProjectContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Vyhledávání
    /// </summary>
    [HttpGet("search")]
    public ActionResult Search(string searchQuery)
    {
        if (searchQuery == string.Empty) return View();
        var projects = _context?.projects?.ToList().Where(project => project.name.Contains(searchQuery));
        return View(projects);
    }
}