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
        if(!UniversalHelper.CheckAllParams(data)) return Json("ERROR");
        // Todo error message
        if (UniversalHelper.GetData<DialCode>(_context, _cache, UniversalHelper.DialCodeCacheKey, "dialCodes").Any(x => x.name == data.Name)) return Json("ERROR");

        var color = ColorTranslator.FromHtml(data?.Color);
        var dialInfo = _context.dialInfos.FirstOrDefault(x => x.name == data.DialInfoName);
        if (dialInfo == null) return Json("Není taková kategorie");
        DialCode dialCode = new(data?.Name, dialInfo, color, data.Description);

        _context.dialCodes.Add(dialCode);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialCodeCacheKey, _context.dialCodes.ToList());
        return Redirect("/project/create");
    }

    [HttpPost("dialinfo/add")]
    public ActionResult AddDialInfo([FromForm] DialInfoData? data)
    {
        // Todo error message
        if (UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos").Any(x => x.name == data.name)) return Json("ERROR");
        // Todo error message
        if(!UniversalHelper.CheckAllParams(data)) return Json("ERROR");

        // Vytvoření kategorie
        DialInfo dialInfo = new()
        {
            name = data?.name,
            desc = data?.description
        };
        _context.dialInfos.Add(dialInfo);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialInfoCacheKey, _context.dialInfos.ToList());
        return Redirect("/project/create");
    }


    [HttpPost("dialinfo/edit/{id}")]
    public ActionResult UpdateDialInfo(int id, [FromForm] DialInfoData? data)
    {
        if (!UniversalHelper.CheckAllParams(data, UniversalHelper.NoCheckUserDataParams)) return Json("ERR");

        var dialInfo = UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos").FirstOrDefault(x => x.id == id);

        dialInfo.desc = data.description;
        dialInfo.name = data.name;
        
        _context.dialInfos.Update(dialInfo);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialInfoCacheKey, _context.dialInfos.ToList());
        return Redirect("/user/profile/");
    }

    [HttpPost("dialcode/edit/{id}")]
    public ActionResult UpdateDialCode(int id, [FromForm] DialCodeData? data)
    {
        if (!UniversalHelper.CheckAllParams(data)) return Json("ERROR");

        var dialCode = UniversalHelper.GetData<DialCode>(_context, _cache, UniversalHelper.DialCodeCacheKey, "dialCodes")?.FirstOrDefault(x => x.id == id);

        var color = ColorTranslator.FromHtml(data?.Color);

        var dialInfo = _context.dialInfos.FirstOrDefault(x => x.name == data.DialInfoName);
        if (dialInfo == null) return Json("Není taková kategorie");

        dialCode.description = data.Description;
        dialCode.color = color;
        dialCode.name = data.Name;
        dialCode.dialInfo = dialInfo;

        _context.dialCodes.Update(dialCode);
        _context.SaveChanges();

        _cache.Set(UniversalHelper.DialCodeCacheKey, _context.dialCodes.ToList());
        return Redirect("/user/profile/");
    }
}