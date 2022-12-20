using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EvidenceProject.Controllers;

public class HomeController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly ProjectContext _context;

    public HomeController(ProjectContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IActionResult Index()
    {
        var projects = UniversalHelper.GetData<Project>(_context, _cache, "AllProjects", "project", true);
        return View(projects);
    }
}