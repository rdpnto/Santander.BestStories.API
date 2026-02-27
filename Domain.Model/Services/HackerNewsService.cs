using Santander.BestStories.Domain.Entities;
using Santander.BestStories.Domain.Interfaces;

namespace Santander.BestStories.Domain.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        public Task<IReadOnlyList<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
