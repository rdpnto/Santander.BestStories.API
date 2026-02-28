using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Contracts.Repositories
{
    public interface IHackerNewsRepository
    {
        Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

        Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
