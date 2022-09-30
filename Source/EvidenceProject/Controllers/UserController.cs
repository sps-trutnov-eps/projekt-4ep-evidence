using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using EvidenceProject.Helpers;

namespace EvidenceProject.Controllers;
public class UserController : Controller
{
    private readonly ProjectContext _context;

    public UserController(ProjectContext context)
    {
        _context = context;
    }

    
    [HttpGet("admin")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString(UniversalHelper.LoggedInKey) != "1")  return Redirect("/");
        return View();
    }

    // <summary>
    // Login view (get)
    // </summary>
    [HttpGet("users/login")]
    public IActionResult Login() => View();
    
    // <summary>
    // Login (post)
    // </summary>
    [HttpPost("users/login")]
    public IActionResult LoginPost([FromForm] LoginData data) 
    {
        AuthUser? user = _context.globalUsers?.FirstOrDefault(u => u.username == data.username);
        if (user == null) return Json(UniversalHelper.SomethingWentWrongMessage);

        if (data.password == null || data.username == null) return Json(UniversalHelper.SomethingWentWrongMessage);

        if (!PasswordHelper.VerifyHash(data.password, user.password)) return Json(UniversalHelper.SomethingWentWrongMessage);

        HttpContext.Session.SetString(UniversalHelper.LoggedInKey, user.id.ToString());
        return Redirect("/");
    }


    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/register")]
    public IActionResult Register() => View();

    // <summary>
    // Register (post)
    // </summary>
    [HttpPost("users/register")]
    public IActionResult RegisterPost([FromForm] LoginData data)
    {
        if (data.username == null || data.password == null) return Json(UniversalHelper.SomethingWentWrongMessage);

        var isUserExisting = _context?.globalUsers?.Any(u => u.username == data.username);
        if ((bool)isUserExisting) return Json("Uživatel již existuje"); // Don't allow 2 users with the same name

        var isFirstUser = _context?.globalUsers?.Any(); // if there is no user 
        var newUser = new AuthUser()
        {
            // TODO add input in view for full name and studyField 
            fullName = data.username,
            studyField = null,

            password = PasswordHelper.CreateHash(data.password),
            username = data.username,
            globalAdmin = !isFirstUser
        };

        _context?.globalUsers?.Add(newUser);
        _context?.SaveChanges();
        return Redirect("/users/login");
    }
}