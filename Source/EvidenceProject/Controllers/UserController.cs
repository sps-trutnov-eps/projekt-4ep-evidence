using EvidenceProject.Controllers.ActionData;
using EvidenceProject.Controllers.RequestClasses;
using Microsoft.EntityFrameworkCore;
using bcrypt = BCrypt.Net.BCrypt;
namespace EvidenceProject.Controllers;

public class UserController : Controller
{
    private readonly ProjectContext _context;
    private readonly ILogger _logger;
    CustomMessageResponse messageResponse = new();
    CustomMessageResponse GetMessageResponse = new() { Response = null };
    public UserController(ProjectContext context, ILogger<UserController> logger)
    {
        _logger = logger;
        _context = context;
    }

    // <summary>
    // Login view (get)
    // </summary>
    [HttpGet("users/login")]
    public ActionResult Login() => View(GetMessageResponse);
    
    // <summary>
    // Login (post)
    // </summary>
    [HttpPost("users/login")]
    public ActionResult LoginPost([FromForm] LoginData data, bool testing = false)
    {
        AuthUser? user = _context.globalUsers?.ToList().FirstOrDefault(u => u.username == data.username);
        if (user == null) return View("Login", messageResponse);

        if (!UniversalHelper.CheckAllParams(data)) return View("Login",messageResponse);

        if(!bcrypt.Verify(data.password, user.password)) return View("Login", messageResponse);

        _logger.LogInformation("{0} logged in.", data.username);
        if (testing) return Redirect("/");
        HttpContext.Session.SetString(UniversalHelper.LoggedInKey, user.id.ToString());
        return Redirect("/");
    }

    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/register")]
    public ActionResult Register() => View(GetMessageResponse);

    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/password/update")]
    public ActionResult PasswordUpdate()
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID)) return Redirect("/");
        return View(GetMessageResponse);
    }


    // <summary>
    // Register (post)
    // </summary>
    [HttpPost("users/register")]
    public ActionResult RegisterPost([FromForm] LoginData data)
    {
        if (!UniversalHelper.CheckAllParams(data)) return View("Register", messageResponse);
        var contextList = _context?.globalUsers?.ToList();

        var doesUserExist = contextList.Any(u => u.username == data.username);
        if (doesUserExist)
        {
            _logger.LogInformation("Someone tried registering with an exisiting username.");
            return View("Register", messageResponse); // Don't allow 2 users with the same name
        }

        var isFirstUser = contextList.Any(); // if there is no user 
        var hashedPassword = bcrypt.HashPassword(data.password);
        var newUser = new AuthUser()
        {
            fullName = data.username,
            username = data.username,
            password = hashedPassword,
            studyField = null,
            contactDetails = null,
            globalAdmin = !isFirstUser
        };
        _logger.LogInformation("A new user created - username: {0}, fullName: {1}, globalAdmin: {2}", newUser.username,
            newUser.fullName, newUser.globalAdmin);

        _context?.globalUsers?.Add(newUser);
        _context?.SaveChanges();
        return Redirect("/users/login");
    }

    [HttpPost("users/password/update")] 
    public ActionResult UpdatePasswordPost([FromForm] LoginDataUpdate data)
    {
        if(!UniversalHelper.CheckAllParams(data)) return View("PasswordUpdate", messageResponse);
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != null && userID != "1") return View("PasswordUpdate", messageResponse);

        AuthUser? user = AuthUser.FindUser(_context, int.Parse(userID));

        if (user == null) return View("PasswordUpdate", messageResponse);

        if(data.new_password_again != data.new_password) return View("PasswordUpdate", messageResponse);
        user.password = bcrypt.HashPassword(data.new_password);
        _context.SaveChanges();

        return Redirect("/");
    }


    [HttpGet("user/profile")]
    public ActionResult Profile()
    {
        if(!UniversalHelper.GetLoggedUser(HttpContext, out string id)) return Redirect("/");
        var userData = _context.globalUsers.FirstOrDefault(x => x.id == int.Parse(id));
        userData.Projects = UniversalHelper.GetProjectsWithIncludes(_context)?.Where(x => x.projectManager.id == int.Parse(id)).ToList();
        return View(userData);
    }

    [HttpPost("user/logout")]
    public ActionResult LogOut()
    {
        HttpContext.Session.Remove(UniversalHelper.LoggedInKey);
        return Json("OK");
    }
}