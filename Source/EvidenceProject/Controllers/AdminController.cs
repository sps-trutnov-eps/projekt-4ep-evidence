using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        // <summary>
        // Login pro admina (get)
        // </summary>
        [HttpGet("admin/login")]
        public IActionResult Login() 
        {
            return View();
        }

        // <summary>
        // Login pro admina (post)
        // </summary>
        [HttpPost("admin/login")]
        public IActionResult LoginPost([FromForm] LoginForm data) 
        {
            if (data.password == "heslo") {
                 HttpContext.Session.SetString("loggedin", "true");
            }
            return Redirect("/admin");
        }
    }
}