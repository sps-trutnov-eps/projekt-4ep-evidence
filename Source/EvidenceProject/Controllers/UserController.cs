using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Helpers;

namespace EvidenceProject.Controllers;
public class AdminController : Controller
{
    private readonly ProjectContext _context;

    public AdminController(ProjectContext context)
    {
        _context = context;
    }
    // <summary>
    // Stranka admina  
    // </summary>
    [HttpGet("admin")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("loggedin") != "true") 
            return Redirect("/users/login");
        return View();
    }

    // <summary>
    // Login view (get)
    // </summary>
    [HttpGet("users/login")]
    public IActionResult Login() 
    {
        return View();
    }

    // <summary>
    // Login (post)
    // </summary>
    [HttpPost("users/login")]
    public IActionResult LoginPost([FromForm] LoginData data) 
    {
        // TODO
        if (data.password == "heslo") {
            HttpContext.Session.SetString(UniversalHelper.LoggedInKey, "true");
            return Redirect("/");
        }
        return Redirect("/users/login");
    }


    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/register")]
    public IActionResult RegisterGet()
    {
        return View();
    }

    // <summary>
    // Register (post)
    // </summary>
    [HttpPost("users/register")]
    public IActionResult RegisterPost([FromForm] LoginData data)
    {
        // TODO

        return Redirect("/user/login");
    }
}