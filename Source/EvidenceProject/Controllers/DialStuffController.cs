using System.Drawing;
using EvidenceProject.Controllers.ActionData;
using Microsoft.Extensions.Caching.Memory;

namespace EvidenceProject.Controllers;

public class DialStuffController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly ProjectContext _context;

    public DialStuffController(ProjectContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpPost("dialcode/add")]
    public ActionResult AddDialCode([FromForm] DialCodeData? data)
    {
        
        if (!UniversalHelper.CheckAllParams(data))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            return Redirect("/project/create");
        }
        
        if (UniversalHelper.GetData<DialCode>(_context, _cache, UniversalHelper.DialCodeCacheKey, "dialCodes").Any(x => x.name == data.Name))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Taková položka již existuje");
            return Redirect("/project/create");
        }
        var color = ColorTranslator.FromHtml(data?.Color);
        var dialInfo = _context.dialInfos.FirstOrDefault(x => x.name == data.DialInfoName);
        if (dialInfo == null)
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Taková kategorie není");
            return Redirect("/project/create");
        }
        DialCode dialCode = new(data?.Name, dialInfo, color, data.Description);

        _context.dialCodes.Add(dialCode);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialCodeCacheKey, _context.dialCodes.ToList());
        return Redirect("/project/create");
    }

    [HttpPost("dialinfo/add")]
    public ActionResult AddDialInfo([FromForm] DialInfoData? data)
    {
       
        if (UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos").Any(x => x.name == data.name))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Taková kategorie existuje");
            return Redirect("/project/create");
        }

        
        if (!UniversalHelper.CheckAllParams(data))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            return Redirect("/project/create");
        }

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
        if (!UniversalHelper.CheckAllParams(data))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            return Redirect("/user/profile");
        }

        var dialInfo = UniversalHelper.GetData<DialInfo>(_context, _cache, UniversalHelper.DialInfoCacheKey, "dialInfos").FirstOrDefault(x => x.id == id);

        if(dialInfo == null)
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Takový kategorie není");
            return Redirect("/user/profile");  
        }

        dialInfo.desc = data.description;
        dialInfo.name = data.name;
        
        _context.dialInfos.Update(dialInfo);
        _context.SaveChanges();
        _cache.Set(UniversalHelper.DialInfoCacheKey, _context.dialInfos.ToList());
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
        return Redirect("/user/profile/");
    }

    [HttpPost("dialcode/edit/{id}")]
    public ActionResult UpdateDialCode(int id, [FromForm] DialCodeData? data)
    {
        if (!UniversalHelper.CheckAllParams(data))
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            return Redirect("/user/profile");
        }

        var dialCode = UniversalHelper.GetData<DialCode>(_context, _cache, UniversalHelper.DialCodeCacheKey, "dialCodes")?.FirstOrDefault(x => x.id == id);

        var color = ColorTranslator.FromHtml(data?.Color);

        var dialInfo = _context.dialInfos.FirstOrDefault(x => x.name == data.DialInfoName);

        if (dialInfo == null)
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Takový kategorie není");
            return Redirect("/user/profile");
        }

        dialCode.description = data.Description;
        dialCode.color = color;
        dialCode.name = data.Name;
        dialCode.dialInfo = dialInfo;

        _context.dialCodes.Update(dialCode);
        _context.SaveChanges();

        _cache.Set(UniversalHelper.DialCodeCacheKey, _context.dialCodes.ToList());
        UniversalHelper.UpdateProjectsInCache(_cache, _context);
        return Redirect("/user/profile/");
    }
}