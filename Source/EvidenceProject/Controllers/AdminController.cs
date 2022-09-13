using EvidenceProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public IActionResult LoginPost([FromForm] object data) 
        {
            return Redirect("/admin");
        }
    }
}