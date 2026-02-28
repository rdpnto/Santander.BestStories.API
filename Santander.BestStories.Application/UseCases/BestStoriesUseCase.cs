using Santander.BestStories.Application.Interfaces;
using Santander.BestStories.Domain.Contracts.Services;
using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Application.UseCases
{
    public class BestStoriesUseCase : IBestStoriesUseCase
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly int _maxConcurrentTasks = 20;

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

            // Using Semaphore for better performance
            // limiting max concurrent tasks to avoid overloading hackernews api

            using var semaphore = new SemaphoreSlim(_maxConcurrentTasks, _maxConcurrentTasks);

            var tasks = ids.Select(async id =>
            {
                await semaphore.WaitAsync(cancellationToken);

                try
                {
                    return await _hackerNewsService.GetStoryByIdAsync(id, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            var stories = await Task.WhenAll(tasks);

            return stories
                .Where(s => s != null)
                .OrderByDescending(s => s.Score)
                .Take(numberOfStories);
        }
    }
}
