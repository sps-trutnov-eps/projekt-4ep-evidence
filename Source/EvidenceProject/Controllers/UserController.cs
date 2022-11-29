using EvidenceProject.Controllers.ActionData;
using EvidenceProject.Controllers.RequestClasses;
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
    // Admin
    // </summary>
    [HttpGet("admin")]
    public ActionResult Index()
    {
        var isLoggedIn = HttpContext.Session.GetString(UniversalHelper.LoggedInKey) == "1";
        if (!isLoggedIn)
        {
            _logger.LogInformation("An unauthorized user tried accessing the admin panel.");
            return Redirect("/users/login");
        }

        return View();
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

        //if (!PasswordHelper.VerifyHash(data.password, user.password)) return Json(UniversalHelper.SomethingWentWrongMessage);
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
        if (!UniversalHelper.GetLoggedUser(HttpContext, out var userID) && userID != null && userID != "1") return Json("Nejsi přihlášen");
        if (!int.TryParse(userID, out int user_id)) return Json("Id neni cisilko");
        AuthUser? user = AuthUser.FindUser(_context, user_id);

        if (user == null) return Json("Uživatel neexistuje?!");
        if (user.password != bcrypt.HashPassword(data.original_password)) return Json("Špatné heslo");

        user.password = bcrypt.HashPassword(data.password);
        _context.SaveChanges();

        return Json("Heslo změněno úspěšně!");
    }
}