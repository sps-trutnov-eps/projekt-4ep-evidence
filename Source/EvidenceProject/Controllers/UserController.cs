using Microsoft.AspNetCore.Mvc;
using EvidenceProject.Controllers.RequestClasses;

namespace EvidenceProject.Controllers
{
    public class AdminController : Controller
    {
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
        public IActionResult LoginPost([FromForm] LoginForm data) 
        {
            // TODO
            if (data.password == "heslo") {
                HttpContext.Session.SetString("loggedin", "true");
                return Redirect("/");
            }
            return Redirect("/users/login");
        }


        // <summary>
        // Register view (get)
        // </summary>
        [HttpPost("users/register")]
        public IActionResult RegisterGet()
        {
            return View();
        }


        // <summary>
        // Register (post)
        // </summary>
        [HttpPost("users/register")]
        public IActionResult RegisterPost([FromForm] LoginForm data)
        {
            // TODO
            return Redirect("/user/login");
        }
    }
}