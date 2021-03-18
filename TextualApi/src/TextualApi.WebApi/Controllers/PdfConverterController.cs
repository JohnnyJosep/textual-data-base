using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextualApi.Application.Handlers.PdfConverter.Commands.PdfConvert;
using TextualApi.WebApi.Controllers.Base;

namespace TextualApi.WebApi.Controllers
{
    public class PdfConverterController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Hello() => Ok("hello");
        
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var result = await Mediator.Send(new PdfConvertCommand {PdfBytes = ms.ToArray()});
            return Ok(result);
        }
    }
}