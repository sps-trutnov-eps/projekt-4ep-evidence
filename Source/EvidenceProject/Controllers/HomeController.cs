namespace EvidenceProject.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/search/{searchData}")]
    public ActionResult Search(string searchData)
    {
        return Json(searchData);
    }
}