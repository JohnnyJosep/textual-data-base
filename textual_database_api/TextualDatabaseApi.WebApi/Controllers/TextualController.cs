using Microsoft.AspNetCore.Mvc;

namespace TextualDatabaseApi.WebApi.Controllers
{
    public class TextualController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}