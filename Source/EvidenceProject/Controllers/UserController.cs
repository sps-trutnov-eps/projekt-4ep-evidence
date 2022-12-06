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
        if (_context.globalUsers?.Count() == 0) { _context.globalUsers.Add(new AuthUser("admin", "heslo", _context)); _context.SaveChanges();}

        AuthUser? user = _context.globalUsers?.ToList().FirstOrDefault(u => u.username == data.username);
        if (user == null) return View("Login", messageResponse);

        if (!UniversalHelper.CheckAllParams(data)) return View("Login",messageResponse);

        if(!bcrypt.Verify(data.password, user.password)) return View("Login", messageResponse);

        _logger.LogInformation("{0} logged in.", data.username);
        if (testing) return Redirect("/");
        HttpContext.Session.SetInt32(UniversalHelper.LoggedInKey, user.id);
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
    public ActionResult RegisterPost([FromForm] RegisterData data)
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
            fullName = $"{data.firstname} {data.lastname}",
            username = data.username,
            password = hashedPassword,
            studyField = data.studyField,
            contactDetails = data.contact,
            schoolYear = byte.Parse(data.schoolYear),
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
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != null && userID != 1) return View("PasswordUpdate", messageResponse);

        AuthUser? user = AuthUser.FindUser(_context, (int)userID);

        if (user == null) return View("PasswordUpdate", messageResponse);

        if(data.new_password_again != data.new_password) return View("PasswordUpdate", messageResponse);
        user.password = bcrypt.HashPassword(data.new_password);
        _context.SaveChanges();

        return Redirect("/");
    }

    [HttpGet("user/profile")]
    public ActionResult Profile()
    {
        if(!UniversalHelper.GetLoggedUser(HttpContext, out var id)) return Redirect("/");
        ProfileData profileData = new();
        var userId = (int)id;

        profileData.AuthUser= _context.globalUsers.FirstOrDefault(x => x.id == userId);
        bool isAdmin = profileData.AuthUser.globalAdmin.Value;
        profileData.Users = isAdmin ? _context.globalUsers.ToList() : null;
        profileData.Categories = isAdmin ? _context.dialInfos.Include(x => x.dialCodes).ToList() : null;
        profileData.Projects = isAdmin ? UniversalHelper.GetProjectsWithIncludes(_context) : UniversalHelper.GetProjectsWithIncludes(_context).Where(x => x.assignees.Any(x=> x.id == userId) || x.projectManager.id == userId).ToList();
        return View(profileData);
    }

    [HttpGet("user/logout")]
    public ActionResult Logout()
    {
        HttpContext.Session.Remove(UniversalHelper.LoggedInKey);
        return RedirectToAction("Index", controllerName: "Home");
    }
}