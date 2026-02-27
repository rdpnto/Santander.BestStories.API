using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Infrastructure.Repositories
{
    public class HackerNewsRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public HackerNewsRepository(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IReadOnlyList<int>> GetBestStoryIdsAsync(CancellationToken ct)
        {
            return await _cache.GetOrCreateAsync("beststories_ids", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var response = await _httpClient.GetAsync("beststories.json", ct);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync(ct);
                return JsonSerializer.Deserialize<List<int>>(content) ?? new List<int>();
            }) ?? new List<int>();

            //var response = await _httpClient.GetAsync("beststories.json", ct);
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync(ct);
            //return JsonSerializer.Deserialize<List<int>>(content) ?? new List<int>();
        }

        public async Task<Story> GetStoryByIdAsync(int id, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"item/{id}.json", ct);
            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync(ct);

            return JsonSerializer.Deserialize<Story>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
