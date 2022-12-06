using EvidenceProject.Controllers.ActionData;
using EvidenceProject.Controllers.RequestClasses;
using Microsoft.EntityFrameworkCore;
using bcrypt = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Caching.Memory;
namespace EvidenceProject.Controllers;

public class UserController : Controller
{
    private readonly ProjectContext _context;
    private readonly ILogger _logger;
    private readonly IMemoryCache _cache;
    CustomMessageResponse messageResponse = new();
    CustomMessageResponse GetMessageResponse = new() { Response = null };
    public UserController(ProjectContext context, ILogger<UserController> logger, IMemoryCache cache)
    {
        _logger = logger;
        _context = context;
        _cache = cache;
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
        var userId = id.Value;

        var projectsWithIncludes = UniversalHelper.GetData<Project>(_context, _cache, "AllProjects", "projects", true);
        List<User> users = new();
        foreach (var project in projectsWithIncludes)
        {
            if(project.assignees != null) foreach (var assignee in project?.assignees) users.Add(assignee);
            if(project.applicants != null) foreach (var applicant in project?.applicants) users.Add(applicant);
        }
        profileData.AuthUser = _context.globalUsers.FirstOrDefault(x => x.id == userId);
        var userProjects = projectsWithIncludes.Where(x => x.assignees?.ToList().Any(x => x.id == userId) == true || x.projectManager.id == userId);
        var userProjectsData = userProjects == null ? null : userProjects.ToList();

        bool isAdmin = profileData.AuthUser.globalAdmin.Value;
        profileData.NonAuthUsers = isAdmin ? users : null;
        profileData.Users = isAdmin ? _context.globalUsers.ToList() : null;
        profileData.Categories = isAdmin ? _context.dialInfos.Include(x => x.dialCodes).ToList() : null;
        profileData.Projects = isAdmin ? projectsWithIncludes : userProjectsData;
        profileData.AuthUser = _context.globalUsers.FirstOrDefault(x => x.id == userId);
        return View(profileData);
    }

    [HttpGet("user/logout")]
    public ActionResult Logout()
    {
        HttpContext.Session.Remove(UniversalHelper.LoggedInKey);
        return RedirectToAction("Index", controllerName: "Home");
    }


    [HttpPost("user/edit/{id}")]
    public ActionResult ChangeUser(int id, RegisterData data)
    {
        if (!UniversalHelper.CheckAllParams(data)) return Redirect("/user/profile");

        if (!UniversalHelper.GetLoggedUser(HttpContext, out int? Uid)) return Redirect("/");

        var loggedId = Uid.Value;

        var user = _context.globalUsers.FirstOrDefault(x => x.id == loggedId);
        var admin = _context.globalUsers.FirstOrDefault(x => x.globalAdmin.Value);

        if (loggedId != user.id && admin.id != loggedId) return Redirect("/user/profile");

        var hashedPassword = bcrypt.HashPassword(data.password);

        user.fullName = $"{data.firstname} {data.lastname}";
        user.username = data.username;
        user.password = hashedPassword;
        user.studyField = data.studyField;
        user.contactDetails = data.contact;
        user.schoolYear = byte.Parse(data.schoolYear);

        _context.globalUsers.Update(user);
        _context.SaveChanges();

        return Redirect("/user/profile");
    }
}