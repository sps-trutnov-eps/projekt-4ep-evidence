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
    public ActionResult Login() => View(new LoginData());
    
    // <summary>
    // Login (post)
    // </summary>
    [HttpPost("users/login")]
    public ActionResult LoginPost([FromForm] LoginData data, bool testing = false)
    {
        if (_context.globalUsers?.Count() == 0) { _context.globalUsers.Add(new AuthUser("admin", "heslo", _context)); _context.SaveChanges();}

        AuthUser? user = _context.globalUsers?.ToList().FirstOrDefault(u => u.username == data.Username);
        if (user == null) return View("Login", data.SetError());

        if (!UniversalHelper.CheckAllParams(data, UniversalHelper.NoCheckUserDataParams)) return View("Login", data.SetError());

        if(!bcrypt.Verify(data.Password, user.password)) return View("Login", data.SetError());

        _logger.LogInformation("{0} logged in.", data.Username);
        if (testing) return Redirect("/");
        HttpContext.Session.SetInt32(UniversalHelper.LoggedInKey, user.id);
        HttpContext.Session.SetInt32(UniversalHelper.IsAdmin, ((bool)user.globalAdmin ? 1 : 0));
        return Redirect("/");
    }

    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/register")]
    public ActionResult Register() => View(new RegisterData());

    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/password/update")]
    public ActionResult PasswordUpdate()
    {
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID)) return Redirect("/");
        return View(new LoginDataUpdate());
    }


    // <summary>
    // Register (post)
    // </summary>
    [HttpPost("users/register")]
    public ActionResult RegisterPost([FromForm] RegisterData data)
    {
        if (!UniversalHelper.CheckAllParams(data, UniversalHelper.NoCheckUserDataParams)) return View("Register", data.SetError());
        var contextList = _context?.globalUsers?.ToList();

        var doesUserExist = contextList.Any(u => u.username == data.Username);
        if (doesUserExist)
        {
            _logger.LogInformation("Someone tried registering with an exisiting username.");
            return View("Register", data.SetError()); // Don't allow 2 users with the same name
        }

        var isFirstUser = contextList.Any(); // if there is no user 
        var hashedPassword = bcrypt.HashPassword(data.Password);
        var newUser = new AuthUser()
        {
            fullName = $"{data.Firstname} {data.Lastname}",
            username = data.Username,
            password = hashedPassword,
            studyField = data.StudyField,
            contactDetails = data.Contact,
            schoolYear = byte.Parse(data.SchoolYear),
            globalAdmin = !isFirstUser
        };
        _logger.LogInformation("A new user created - username: {0}, fullName: {1}, globalAdmin: {2}", newUser.username,
            newUser.fullName, newUser.globalAdmin);

        _context?.globalUsers?.Add(newUser);
        _context?.SaveChanges();
        return Redirect("/users/login");
    }

    [HttpPost("user/password/update")] 
    public ActionResult UpdatePasswordPost([FromForm] LoginDataUpdate data)
    {
        if(!UniversalHelper.CheckAllParams(data, UniversalHelper.NoCheckUserDataParams)) return View("PasswordUpdate", data.SetError());
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context, out var userID)) return View("PasswordUpdate", data.SetError());

        AuthUser? user = AuthUser.FindUser(_context, (int)userID);

        if (user == null) return View("PasswordUpdate", data.SetError());

        if(data.new_password_again != data.new_password) return View("PasswordUpdate", data.SetError());
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
        HttpContext.Session.Remove(UniversalHelper.IsAdmin);
        return RedirectToAction("Index", controllerName: "Home");
    }


    [HttpPost("user/edit/{id}")]
    public ActionResult ChangeUser(int id, RegisterData data)
    {
        if (!UniversalHelper.CheckAllParams(data, UniversalHelper.NoCheckUserDataParams)) return Redirect("/user/profile");

        if (!UniversalHelper.GetLoggedUser(HttpContext, out int? Uid)) return Redirect("/");

        var loggedId = Uid.Value;

        var user = _context.globalUsers.FirstOrDefault(x => x.id == loggedId);
        var admin = _context.globalUsers.FirstOrDefault(x => x.globalAdmin.Value);

        if (loggedId != user.id && admin.id != loggedId) return Redirect("/user/profile");

        var hashedPassword = bcrypt.HashPassword(data.Password);

        user.fullName = $"{data.Firstname} {data.Lastname}";
        user.username = data.Username;
        user.password = hashedPassword;
        user.studyField = data.StudyField;
        user.contactDetails = data.Contact;
        user.schoolYear = byte.Parse(data.SchoolYear);

        _context.globalUsers.Update(user);
        _context.SaveChanges();

        return Redirect("/user/profile");
    }

    [HttpGet("user/delete/{id}")]
    public ActionResult DeleteUser(int id)
    {
        // Todo dodělat kontrolu admina
        var user = _context.globalUsers.FirstOrDefault(u => u.id == id);
        if(user == null) Redirect("/user/profile");

        _context.globalUsers.Remove(user);
        _context.SaveChanges();
        return Redirect("/user/profile");
    }
}