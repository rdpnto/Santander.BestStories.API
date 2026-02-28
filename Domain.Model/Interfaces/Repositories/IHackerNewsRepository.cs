using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Interfaces.Repositories
{
    public interface IHackerNewsRepository
    {
        Task<IEnumerable<uint>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

        Task<Story> GetStoryByIdAsync(uint id, CancellationToken cancellationToken);
    }
}
