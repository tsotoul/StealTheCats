using Microsoft.AspNetCore.Mvc;
using StealTheCats.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace StealTheCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly ICatsService _catsService;

        public CatsController(ICatsService _catsService)
        {
            this._catsService = _catsService;
        }

        [HttpPost("fetch")]
        [SwaggerOperation(Summary = "Fetch Cats from The Cat Api", Description = "Fetches 25 cats and their attributes from thecatapi.com and saves them to the database")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.Created, Description = "Fetched and saved 25 cats successfully.")]
        public async Task<IActionResult> FetchCatsAsync()
        {
            await _catsService.FetchCatsAsync();
            return new CreatedResult("", true);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets Cats from the database", Description = "Gets 10 cats from the database")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Description = "Gets paginated cats from database successfully.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.BadRequest, Description = "Invalid client input data")]
        public async Task<IActionResult> GetCatsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var cats = await _catsService.GetCatsAsync(page, pageSize);

            return Ok(cats);
        }

        [HttpGet("byTag")]
        [SwaggerOperation(Summary = "Gets Cats from the database by specific tag", Description = "Gets 10 cats from the database by specific tag")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Description = "Gets paginated cats from database by specific tag successfully.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.BadRequest, Description = "Invalid client input data")]
        public async Task<IActionResult> GetCatsByTagAsync([FromQuery] string tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || tag == "")
            {
                return BadRequest("There is a problem with the parameters. Please check that page and pageSize are > 0 and tag is not empty");
            }

            var cats = await _catsService.GetCatsByTagAsync(tag, page, pageSize);
            return Ok(cats);
        }
    }
}
