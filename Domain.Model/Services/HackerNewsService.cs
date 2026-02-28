using Santander.BestStories.Domain.Entities;
using Santander.BestStories.Domain.Interfaces.Repositories;
using Santander.BestStories.Domain.Interfaces.Services;

namespace Santander.BestStories.Domain.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsRepository _hackerNewsRepository;

        public HackerNewsService(IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRepository = hackerNewsRepository ?? throw new ArgumentNullException(nameof(hackerNewsRepository));
        }

        public Task<IEnumerable<uint>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
        {
            return _hackerNewsRepository.GetBestStoryIdsAsync(cancellationToken);
        }

        public Task<Story> GetStoryByIdAsync(uint id, CancellationToken cancellationToken)
        {
            return _hackerNewsRepository.GetStoryByIdAsync(id, cancellationToken);
        }
    }
}
