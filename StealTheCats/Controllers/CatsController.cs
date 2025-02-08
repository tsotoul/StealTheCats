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
        [SwaggerResponse(statusCode: (int)HttpStatusCode.BadRequest, Description = "Invalid client input data.")]
        [SwaggerResponse(statusCode: (int)HttpStatusCode.Forbidden, Description = "User not permitted to perform such operation")]
        public async Task<IActionResult> FetchCatsAsync()
        {
            await _catsService.FetchCatsAsync();
            return new CreatedResult("", true);
        }

        // GET api/<Cats2Controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
