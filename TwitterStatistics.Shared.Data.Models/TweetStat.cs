namespace TwitterStatistics.Shared.Data.Models
{
    public class TweetStat
    {
        public int TotalTweetsReceived { get; set; }
        public TweetCount[] TopHashTags { get; set; }
        public TweetCount MostLiked { get; set; }
        public TweetCount MostRetweeted { get; set; }
    }

    public class TweetCount
    {
        public string Text { get; set; }
        public int Count { get; set; }


    }
}