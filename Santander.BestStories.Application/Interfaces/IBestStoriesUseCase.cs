using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Application.Interfaces
{
    public interface IBestStoriesUseCase
    {
        Task<IReadOnlyList<Story>> GetBestStoriesAsync(int numberOfStories, CancellationToken cancellationToken);
    }
}
