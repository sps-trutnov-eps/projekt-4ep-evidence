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
        var projects = _cache.Get("AllProjects");
        if (projects != null) return View(projects);

        projects =_context.projects
            .Include(x => x.projectTechnology)
            .Include(x => x.projectType)
            .ToList();
        _cache.Set("AllProjects", projects);
        return View(projects);
    }
}