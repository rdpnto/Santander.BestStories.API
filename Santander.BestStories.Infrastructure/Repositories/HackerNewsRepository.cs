using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Santander.BestStories.Domain.Contracts.Repositories;
using Santander.BestStories.Domain.Entities;
using Santander.BestStories.Infrastructure.Dtos;

namespace Santander.BestStories.Infrastructure.Repositories
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public HackerNewsRepository(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
        {
            var response = await _cache.GetOrCreateAsync("best_stories_ids", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var response = await _httpClient
                    .GetAsync
                    (
                        requestUri: "beststories.json",
                        cancellationToken: cancellationToken
                    );

                if (!response.IsSuccessStatusCode) return null;

                var content = await response
                    .Content
                    .ReadAsStringAsync(cancellationToken);

                return JsonSerializer
                    .Deserialize<List<int>>(content);
            });

            return response ?? [];
        }

        public async Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _cache.GetOrCreateAsync(id, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var response = await _httpClient
                    .GetAsync
                    (
                        requestUri: $"item/{id}.json",
                        cancellationToken: cancellationToken
                    );

                if (!response.IsSuccessStatusCode) return null;

                var content = await response
                    .Content
                    .ReadAsStringAsync(cancellationToken);

                return JsonSerializer.Deserialize<StoryDto>(content);
            });

            if (response is null) return null;

            return response.ToStory();
        }
    }
}
