using System.Drawing;
using EvidenceProject.Controllers.ActionData;
using Microsoft.Extensions.Caching.Memory;

namespace EvidenceProject.Controllers;

public class DialStuffController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly ProjectContext _context;
    private readonly ILogger<ProjectController> _logger;

    public DialStuffController(ProjectContext context, IMemoryCache cache, ILogger<ProjectController> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    [HttpPost("dialcode/add")]
    public ActionResult AddDialCode([FromForm] DialCodeData? data)
    {
        // Todo error message
        if (_context.dialCodes.Any(x => x.name == data.name)) return Json("ERROR");
        // Todo error message
        if (data?.description.Length == 0 || data?.name.Length == 0 || data.dialInfoName == null) return Json("ERROR");

        var color = Color.FromArgb(data.alpha, data.red, data.green, data.blue);
        var dialInfo = _context.dialInfos.FirstOrDefault(x => x.name == data.name);
        if (dialInfo == null) return Json("Není taková kategorie");
        DialCode dialCode = new()
        {
            name = data.name,
            description = data.description,
            color = color,
            dialInfo = dialInfo
        };
        _context.dialCodes.Add(dialCode);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialCodeCacheKey, _context.dialInfos.ToList());
        return Redirect("/project/create");
    }

    [HttpPost("dialinfo/add")]
    public ActionResult AddDialInfo([FromForm] DialInfoData? data)
    {
        // Todo error message
        if (_context.dialInfos.Any(x => x.name == data.name)) return Json("ERROR");
        // Todo error message
        if (data?.description.Length == 0 || data?.name.Length == 0) return Json("ERROR");

        // Vytvoření kategorie
        DialInfo dialInfo = new()
        {
            name = data.name,
            desc = data.description
        };
        _context.dialInfos.Add(dialInfo);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialInfoCacheKey, _context.dialInfos.ToList());
        return Redirect("/project/create");
    }

    public ActionResult GetDialInfos()
    {
        var cacheData = _cache.Get(UniversalHelper.DialInfoCacheKey);
        if (cacheData != null) return PartialView("DialInfosPV", cacheData);

        var data = _context.dialInfos.ToList();
        _cache.Set(UniversalHelper.DialInfoCacheKey, data);
        return PartialView("DialInfosPV", data);
    }
}