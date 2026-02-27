using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("best")]
        public async Task<IActionResult> GetBestStories(
            [FromQuery] int n = 10,
            CancellationToken ct = default)
        {
            if (n <= 0 || n > 200)
                return BadRequest("n must be between 1 and 200");

            var stories = await _useCase.GetBestStoriesAsync(n, ct);

            return Ok(stories);
        }
    }
}
