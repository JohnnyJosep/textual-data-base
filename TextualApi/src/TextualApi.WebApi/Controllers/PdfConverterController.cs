using Microsoft.AspNetCore.Mvc;

namespace TextualApi.WebApi.Controllers
{
    public class PdfConverterController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}