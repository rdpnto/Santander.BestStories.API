using Santander.BestStories.Domain.Entities;

namespace Santander.BestStories.Infrastructure.Dtos
{
    public static class Mapper
    {
        public static Story ToStory(this StoryDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new Story
            (
                title: dto.title,
                uri: dto.url,
                postedBy: dto.by,
                time: DateTimeOffset.FromUnixTimeSeconds(dto.time),
                score: dto.score,
                commentCount: dto.descendants
            );
        }
    }
}
