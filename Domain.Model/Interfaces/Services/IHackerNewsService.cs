using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Interfaces.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<uint>> GetBestStoryIdsAsync(CancellationToken cancellationToken);

        Task<Story> GetStoryByIdAsync(uint id, CancellationToken cancellationToken);
    }
}
