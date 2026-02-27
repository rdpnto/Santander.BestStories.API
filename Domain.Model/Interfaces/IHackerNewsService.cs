using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Interfaces
{
    public interface IHackerNewsService
    {
        Task<IReadOnlyList<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

        Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
