using EvidenceProject.Controllers.ActionData;
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

    public ActionResult Index()
    {
        var projects = UniversalHelper.GetData<Project>(_context, _cache, "AllProjects", "project", true);
        return View(projects);
    }

    [Route("{*url}", Order = 999)]
    public IActionResult Error404() => View();
}