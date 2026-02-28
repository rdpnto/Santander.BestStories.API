using Santander.BestStories.Domain.Contracts.Repositories;
using Santander.BestStories.Domain.Contracts.Services;
using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Domain.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHackerNewsRepository _hackerNewsRepository;

        public HackerNewsService(IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRepository = hackerNewsRepository ?? throw new ArgumentNullException(nameof(hackerNewsRepository));
        }

        public Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
        {
            return _hackerNewsRepository.GetBestStoryIdsAsync(cancellationToken);
        }

        public Task<Story> GetStoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _hackerNewsRepository.GetStoryByIdAsync(id, cancellationToken);
        }
    }
}
