using FluentAssertions;
using Moq;
using Santander.BestStories.Application.UseCases;
using Santander.BestStories.Domain.Contracts.Services;
using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Tests.Application.UseCases
{
    public class BestStoriesUseCaseTests
    {
        private readonly Mock<IHackerNewsService> _serviceMock;
        private readonly BestStoriesUseCase _useCase;

        public BestStoriesUseCaseTests()
        {
            _serviceMock = new Mock<IHackerNewsService>();
            _useCase = new BestStoriesUseCase(_serviceMock.Object);
        }

        [Fact]
        public async Task GetBestStoriesAsync_Should_Return_Top_N_Stories_Ordered_By_Score()
        {
            IEnumerable<int> ids = new int[] { 1, 2, 3 };

            _serviceMock
                .Setup(s => s.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ids);

            _serviceMock
                .Setup(s => s.GetStoryByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) => new Story
                (
                    title: $"Story {id}",
                    uri: $"https://test.com/{id}",
                    postedBy: "tester",
                    time: DateTimeOffset.UtcNow,
                    score: id * 10,
                    commentCount: id
                ));

            var result = await _useCase.GetBestStoriesAsync(2, CancellationToken.None);

            result.Should().HaveCount(2);
            result.First().Score.Should().BeGreaterThan(result.Last().Score);
            result.Select(x => x.Score).Should().BeInDescendingOrder();
        }

        [Fact]
        public async Task GetBestStoriesAsync_Should_Return_Empty_When_No_Ids()
        {
            _serviceMock
                .Setup(s => s.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            var result = await _useCase.GetBestStoriesAsync(5, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBestStoriesAsync_Should_Return_Exactly_N_Stories()
        {
            var ids = Enumerable.Range(1, 10);

            _serviceMock
                .Setup(s => s.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ids);

            _serviceMock
                .Setup(s => s.GetStoryByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) => new Story
                (
                    title: $"Story {id}",
                    uri: $"https://test.com/{id}",
                    postedBy: "tester",
                    time: DateTimeOffset.UtcNow,
                    score: id * 10,
                    commentCount: id
                ));

            var result = await _useCase.GetBestStoriesAsync(3, CancellationToken.None);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetBestStoriesAsync_Should_Call_Service_For_Each_Id()
        {
            var ids = Enumerable.Range(1, 20);

            _serviceMock
                .Setup(s => s.GetBestStoryIdsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ids);

            _serviceMock
                .Setup(s => s.GetStoryByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken _) => new Story
                (
                    title: $"Story {id}",
                    uri: $"https://test.com/{id}",
                    postedBy: "tester",
                    time: DateTimeOffset.UtcNow,
                    score: id * 10,
                    commentCount: id
                ));

            await _useCase.GetBestStoriesAsync(10, CancellationToken.None);

            _serviceMock.Verify
            (
                expression: s => s.GetStoryByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                times: Times.AtLeastOnce
            );
        }
    }
}
