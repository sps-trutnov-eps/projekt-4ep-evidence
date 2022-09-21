using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Data.SqlClient;

namespace EvidenceProject.Controllers
{
    public class SearchController : Controller
    {
        //Todo - search
        [HttpGet]
        public IActionResult Search()
        {
            return Json("Search ok");
        }
    }
}
