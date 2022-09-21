using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Helpers;

namespace EvidenceProject.Controllers;
public class ProjectController : Controller
{
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
    public ActionResult Create([FromForm] ProjectCreateData request)
    {
        
        return Json("ok");
    }

    [HttpGet("project/create")]
    public ActionResult Create()
    {
        return View();
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
