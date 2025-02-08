using Microsoft.AspNetCore.Mvc;
using StealTheCatsApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace StealTheCatsApi.Controllers
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

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a Cat from the database by Id", Description = "Fetches a cat based on its unique Id")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Description = "Gets cat by Id.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.NotFound, Description = "Cat not found in the database.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.BadRequest, Description = "Invalid client input data")]
        public async Task<IActionResult> GetCatByIdAsync([FromQuery] string catId)
        {
            if (id < 1)
            {
                return BadRequest("ID must be greater than 0.");
            }

            var cat = await _catsService.GetCatByIdAsync(catId);

            return Ok(cat);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets Cats from the database", Description = "Gets paginated cats from the database, optionally filtered by tag")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, Description = "Gets paginated cats from database successfully.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.BadRequest, Description = "Invalid client input data")]
        public async Task<IActionResult> GetCatsAsync([FromQuery] string? tag, [FromQuery] int page, [FromQuery] int pageSize)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var cats = string.IsNullOrEmpty(tag)
                ? await _catsService.GetCatsAsync(page, pageSize)
                : await _catsService.GetCatsByTagAsync(tag, page, pageSize);

            return Ok(cats);
        }
    }
}
