using EvidenceProject.Controllers.ActionData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using bcrypt = BCrypt.Net.BCrypt;

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
        /*
        #if (DEBUG)
            data.Username = "a";
            data.Password = "a";
        #endif
        */
        if (_context.globalUsers?.Count() == 0) { _context.globalUsers.Add(new AuthUser("admin", "heslo", _context)); _context.SaveChanges();}

        AuthUser? user = _context.globalUsers?.ToList().FirstOrDefault(u => u.username == data.Username);
        if (user.IsNull()) return View("Login", data.SetError());

        if (!UniversalHelper.CheckAllParams(data)) return View("Login", data.SetError());

        if(!bcrypt.Verify(data.Password, user.password)) return View("Login", data.SetError());

        _logger.LogInformation("{0} logged in.", data.Username);
        if (testing) return Redirect("/");
        
        HttpContext.Session.SetInt32(UniversalHelper.LoggedInKey, user.id);
        HttpContext.Session.SetString(UniversalHelper.LoggedNameKey, user.username);
        HttpContext.Session.SetInt32(UniversalHelper.IsAdmin, (bool)user.globalAdmin ? 1 : 0);

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
        if (!UniversalHelper.CheckAllParams(data)) return View("Register", data.SetError());
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
        if(!UniversalHelper.CheckAllParams(data)) return View("PasswordUpdate", data.SetError());
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
      
        profileData.AuthUser = _context.globalUsers.FirstOrDefault(x => x.id == userId);
        var userProjects = projectsWithIncludes.Where(x => x.assignees?.ToList().Any(x => x.id == userId) == true || x.projectManager.id == userId);
        bool isAdmin = profileData.AuthUser.globalAdmin.Value;

        profileData.Projects = isAdmin ? projectsWithIncludes : userProjects.ToList();
        profileData.AuthUser = _context?.globalUsers.FirstOrDefault(x => x.id == userId);

        if (!UniversalHelper.TryGetErrorMessage(HttpContext, out var message)) return View(profileData);
        return View(profileData.SetError(message));
    }

    [HttpGet("user/logout")]
    public ActionResult Logout()
    {
        HttpContext.Session.Remove(UniversalHelper.LoggedInKey);
        HttpContext.Session.Remove(UniversalHelper.IsAdmin);
        HttpContext.Session.Remove(UniversalHelper.LoggedNameKey);
        return RedirectToAction("Index", controllerName: "Home");
    }


    [HttpPost("user/edit/{id}")]
    public ActionResult ChangeUser(int id, UpdateUserData data)
    {
        if (!UniversalHelper.CheckAllParams(data)){
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            var uri = HttpContext.Request.Path.Value;
            return Redirect(data.Caller);
        }

        //if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/");

        if (!UniversalHelper.GetLoggedUser(HttpContext, out int? Uid)) return Redirect("/");
        var loggedId = Uid.Value;

        var user = _context.globalUsers.FirstOrDefault(x => x.id == id);
        var admins = _context.globalUsers.Where(x => x.globalAdmin.Value);

        if (loggedId != user.id && !admins.Any(x => x.id == loggedId)) return Redirect(data.Caller);

        user.fullName = $"{data.Firstname} {data.Lastname}";
        user.username = data.Username;
        if (data.Password != null)
            user.password = bcrypt.HashPassword(data.Password);
        user.studyField = data.StudyField;
        user.contactDetails = data.Contact;
        user.schoolYear = byte.Parse(data.SchoolYear);

        _context.globalUsers.Update(user);
        _context.SaveChanges();

        return Redirect(data.Caller);
    }

    [HttpPost("user/delete/{id}")]
    public ActionResult DeleteUser(int id)
    {
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/Administration");
        var user = _context.globalUsers.FirstOrDefault(u => u.id == id);
        if(user.IsNull()) Redirect("/Administration");

        try
        {
            _context.globalUsers.Remove(user);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "K uživateli jsou přiřazeny projekty, není možné jej odstranit.");
            _logger.Log(LogLevel.Warning, ex.Message);
        }
        return Redirect("/Administration");
    }


    [HttpPost("/user/promote/{id}")]
    public ActionResult PromoteUser(int id, [FromForm] RegisterData data)
    {
        if (!UniversalHelper.AuthentifyAdmin(HttpContext, _context)) return Redirect("/Administration");

        if (!UniversalHelper.CheckAllParams(data)) {
            HttpContext.Session.SetString(UniversalHelper.RedirectError, "Něco nebylo vyplněno");
            return Redirect("/Administration");
        }

        var user = _context.users.Include(x => x.Projects).FirstOrDefault(u => u.id == id);

        if (user.IsNull()) return Redirect("/Administration");

        AuthUser authUser = new()
        {
            globalAdmin = false,
            contactDetails = data.Contact,
            fullName = $"{data.Firstname} {data.Lastname}",
            Projects = user.Projects,
            password = BCrypt.Net.BCrypt.HashPassword(data.Password),
            schoolYear = byte.Parse(data.SchoolYear),
            username = data.Username,
            studyField = data.StudyField
        };

        foreach (var project in UniversalHelper.GetProjectsWithIncludes(_context))
        {
            if (project.assignees.IsNullOrEmpty()) continue;
            if (project.assignees.Contains(user))
            {
                project.assignees.Add(authUser);
                project.assignees.Remove(user);
            }
            _context?.projects?.Update(project);

        }

        _context.users.Remove(user);
        _context.globalUsers?.Add(authUser);
        _context.SaveChanges();
        return Redirect("/Administration");
    }
}