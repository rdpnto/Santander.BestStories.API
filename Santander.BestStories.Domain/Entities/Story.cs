namespace Santander.BestStories.Domain.Entities
{
    public class Story
    {
        public Story
        (
            string title,
            string uri,

            string postedBy,
            DateTimeOffset time,

            int score,
            int commentCount
        )
        {
            Title = title;
            Uri = uri;

            PostedBy = postedBy;
            Time = time;

            Score = score;
            CommentCount = commentCount;
        }

        public string Title { get; private set; }
        public string Uri { get; private set; }
        public string PostedBy { get; private set; }
        public DateTimeOffset Time { get; private set; }
        public int Score { get; private set; }
        public int CommentCount { get; private set; }
    }
}
