namespace EvidenceProject.Controllers;
public class UserController : Controller
{
    private readonly ProjectContext _context;
    private readonly ILogger _logger;

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
        bool isLoggedIn = HttpContext.Session.GetString(UniversalHelper.LoggedInKey) == "1";
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
    public ActionResult Login() => View();
    
    // <summary>
    // Login (post)
    // </summary>
    [HttpPost("users/login")]
    public ActionResult LoginPost([FromForm] LoginData data, bool testing = false) 
    {
        AuthUser? user = _context.globalUsers?.ToList().FirstOrDefault(u => u.username == data.username);
        if (user == null) return Json(UniversalHelper.SomethingWentWrongMessage);

        if (data.password == null || data.username == null) return Json(UniversalHelper.SomethingWentWrongMessage);

        if (!PasswordHelper.VerifyHash(data.password, user.password)) return Json(UniversalHelper.SomethingWentWrongMessage);

        _logger.LogInformation("{0} logged in.", data.username);
        if (testing) return Redirect("/"); 
        HttpContext.Session.SetString(UniversalHelper.LoggedInKey, user.id.ToString());
        return Redirect("/");
    }

    // <summary>
    // Register view (get)
    // </summary>
    [HttpGet("users/register")]
    public ActionResult Register() => View();

    // <summary>
    // Register (post)
    // </summary>
    [HttpPost("users/register")]
    public ActionResult RegisterPost([FromForm] LoginData data)
    {
        if (data.username == null || data.password == null) return Json(UniversalHelper.SomethingWentWrongMessage);
        var contextList = _context?.globalUsers?.ToList();

        var doesUserExist = contextList.Any(u => u.username == data.username);
        if (doesUserExist)
        {
            _logger.LogInformation("Someone tried registering with an exisiting username.");
            return Json("Uživatel již existuje"); // Don't allow 2 users with the same name
        }
        var isFirstUser = contextList.Any(); // if there is no user 
        string passwordHash = PasswordHelper.CreateHash(data.password);
        var newUser = new AuthUser()
        {
            fullName = data.username,
            username = data.username,
            password = passwordHash,
            studyField = null,
            contactDetails = null,
            globalAdmin = !isFirstUser
        };
        _logger.LogInformation("A new user created - username: {0}, fullName: {1}, globalAdmin: {2}", newUser.username, newUser.fullName, newUser.globalAdmin);

        _context?.globalUsers?.Add(newUser);
        _context?.SaveChanges();
        return Redirect("/users/login");
    }
}