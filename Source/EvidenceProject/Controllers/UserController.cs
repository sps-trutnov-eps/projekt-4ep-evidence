using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
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
        AuthUser? user = _context.globalUsers?.Where(u => u.username == data.username).First();

        if (user == null)
            return Redirect("/user/login");

        if (data.password == null || user.password == null)
            return Redirect("/user/login");

        if (!PasswordHelper.VerifyHash(data.password, user.password))
            return Redirect("/users/login");

        HttpContext.Session.SetString(UniversalHelper.LoggedInKey, user.id.ToString());
        return Redirect("/");
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
        if (_context.globalUsers?.Where(u => u.username == data.username).Count() > 0)
            return Redirect("/user/register"); // Don't allow 2 users with the same name

        int? userCount = _context.globalUsers?.Count(); // count users => if 0 then user will be declered a globalAdmin
        AuthUser newUser = new AuthUser { password = PasswordHelper.CreateHash(data.password ?? "defaultniheslo"), username = data.username, globalAdmin = userCount == 0 };
        _context.globalUsers?.Add(newUser);
        _context.SaveChanges();
        return Redirect("/user/login");
    }
}