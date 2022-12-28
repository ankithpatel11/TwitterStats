using System.Collections.Concurrent;

namespace TwitterStatistics.Shared.Data.Models
{
    public class Store
    {
        public ConcurrentBag<Rootobject> ConcurrentObjects { get; set; }
    }

    public class Rootobject
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public DateTime created_at { get; set; }
        public string[] edit_history_tweet_ids { get; set; }
        public Entities entities { get; set; }
        public string id { get; set; }
        public string text { get; set; }
        public Public_Metrics public_metrics { get; set; }
    }

    public class Entities
    {
        public Hashtag[] hashtags { get; set; }
    }

    public class Hashtag
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class Public_Metrics
    {
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int like_count { get; set; }
        public int quote_count { get; set; }
    }
}
