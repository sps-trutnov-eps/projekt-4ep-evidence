namespace EvidenceProject.Controllers;
public class HomeController : Controller
{
    private readonly ProjectContext _context;
    public HomeController(ProjectContext context) => _context = context;
    public IActionResult Index() => View();
    
}