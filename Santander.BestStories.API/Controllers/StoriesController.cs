using Microsoft.AspNetCore.Mvc;
using Santander.BestStories.Application.Interfaces;

namespace Santander.BestStories.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IBestStoriesUseCase _useCase;

        public StoriesController(IBestStoriesUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("best-stories")]
        public async Task<IActionResult> GetBestStories
        (
            [FromQuery] int n,
            CancellationToken cancellationToken = default
        )
        {
            if (n <= 0 || n > 200) return BadRequest("n must sit between 1 and 200");

            var stories = await _useCase
                .GetBestStoriesAsync
                (
                    numberOfStories: n,
                    cancellationToken: cancellationToken
                );

            return Ok(stories);
        }
    }
}
