using EvidenceProject.Controllers.ActionData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EvidenceProject.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IMemoryCache _cache;
        public AdministrationController(ProjectContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public IActionResult Index()
        {
            if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/404");

            var administrationData = new AdministrationData();
            var projectsWithIncludes = UniversalHelper.GetData<Project>(_context, _cache, "AllProjects", "projects", true);

            administrationData.Projects = projectsWithIncludes;
            administrationData.NonAuthUsers = _context?.users.Include(x => x.Projects).ToList().Where(x => x.GetType().Name == "User").ToList();
            administrationData.Users = _context?.globalUsers.Include(x => x.Projects).ToList();
            administrationData.Categories = _context?.dialInfos.Include(x => x.dialCodes).ToList();

            if (!UniversalHelper.TryGetErrorMessage(HttpContext, out var message)) return View(administrationData);
            return View(administrationData.SetError(message));
        }
    }
}
