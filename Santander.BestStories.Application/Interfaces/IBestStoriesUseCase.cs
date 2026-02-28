using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Application.Interfaces
{
    public interface IBestStoriesUseCase
    {
        Task<IEnumerable<Story>> GetBestStoriesAsync
        (
            int numberOfStories,
            CancellationToken cancellationToken
        );
    }
}
