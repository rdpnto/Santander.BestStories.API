using Santander.BestStories.Application.Interfaces;
using Santander.BestStories.Domain.Entities;
using Santander.BestStories.Domain.Interfaces.Services;

namespace Santander.BestStories.Application.UseCases
{
    public class BestStoriesUseCase : IBestStoriesUseCase
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly int _concurrentTasksLimit = 20;

        public BestStoriesUseCase(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        public async Task<IEnumerable<Story>> GetBestStoriesAsync
        (
            int numberOfStories,
            CancellationToken cancellationToken
        )
        {
            var ids = (await _hackerNewsService
                .GetBestStoryIdsAsync(cancellationToken))
                .Take(numberOfStories)
                .ToList();

            // implementing Fork-Join design pattern for better performance
            // limiting max concurrent tasks to avoid overloading hackernews api

            var tasks = new List<Task<Story>>();
            var stories = new List<Story>();

            foreach (var id in ids)
            {
                tasks.Add(_hackerNewsService.GetStoryByIdAsync(id, cancellationToken));

                if (tasks.Count >= _concurrentTasksLimit)
                {
                    var batch = await Task.WhenAll(tasks);
                    stories.AddRange(batch);

                    tasks.Clear();
                }
            }

            if (tasks.Count > 0)
            {
                var batch = await Task.WhenAll(tasks);
                stories.AddRange(batch);
            }

            tasks.Clear();
            tasks = null;

            return stories;
        }
    }
}
