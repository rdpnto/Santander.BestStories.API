using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Contracts.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

        Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
