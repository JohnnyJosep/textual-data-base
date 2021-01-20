using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TextualApi.Application.Handlers.Text.Commands.IndexText;
using TextualApi.WebApi.Controllers.Base;

namespace TextualApi.WebApi.Controllers
{
    public class TextController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IndexTextCommand command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }
    }
}