using Santander.BestStories.Application.Interfaces;
using Santander.BestStories.Domain.Entities;
using Santander.BestStories.Domain.Interfaces;

namespace Santander.BestStories.Application.UseCases
{
    public class BestStoriesUseCase : IBestStoriesUseCase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public BestStoriesUseCase(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        public async Task<IReadOnlyList<Story>> GetBestStoriesAsync(int numberOfStories, CancellationToken cancellationToken)
        {
            var ids = await _hackerNewsService.GetBestStoryIdsAsync(cancellationToken);

            var tasks = ids
                .Take(numberOfStories)
                .Select(id => _hackerNewsService.GetStoryByIdAsync(id, cancellationToken));

            var results = await Task.WhenAll(tasks);

            var stories = results
                .Where(x => x != null)
                .Select(item => new Story
                {
                    Title = item!.Title ?? "",
                    Uri = item.Url ?? "",
                    PostedBy = item.By ?? "",
                    Time = DateTimeOffset.FromUnixTimeSeconds(item.Time).UtcDateTime,
                    Score = item.Score,
                    CommentCount = item.Descendants
                })
                .OrderByDescending(x => x.Score)
                .Take(numberOfStories)
                .ToList();

            return stories;
        }
    }
}
