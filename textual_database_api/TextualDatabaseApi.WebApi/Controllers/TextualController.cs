using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TextualDatabaseApi.Application.Commands.SaveTextualData;
using TextualDatabaseApi.Application.Queries.GetTextualData;
using TextualDatabaseApi.WebApi.Controllers.Base;

namespace TextualDatabaseApi.WebApi.Controllers
{
    public class TextualController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetTextualDataRequest();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SaveTextualDataRequest command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}