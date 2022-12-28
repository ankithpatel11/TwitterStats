using MediatR;
using TwitterStatistics.Api.Queries;
using TwitterStatistics.Shared.Data.Models;

namespace TwitterStatistics.Api.QueryHandlers
{
    public class GetTwitterStatsQueryHandler : IRequestHandler<GetTwitterStatsQuery, TweetStat>
    {
        private Store _store;
        public GetTwitterStatsQueryHandler(Store store)
        {
            _store = store;
        }

        public async Task<TweetStat> Handle(GetTwitterStatsQuery request, CancellationToken cancellationToken)
        {
            var res = new TweetStat();
            try
            {
                if (_store.ConcurrentObjects == null || _store.ConcurrentObjects.Count == 0)
                {
                    throw new ArgumentNullException("Datastore is empty");
                }
                res.TotalTweetsReceived = _store.ConcurrentObjects.Count;
                var hashs = _store.ConcurrentObjects.Where(x => x.data.entities != null && x.data.entities.hashtags != null)
                                     .SelectMany(x => x.data.entities.hashtags)
                                     .GroupBy(x => x.tag).Select(x => new { Tag = x.Key, Count = x.Count() })
                                     .OrderByDescending(x => x.Count)
                                     .Take(10);
                if (hashs?.Count() > 0)
                {
                    res.TopHashTags = hashs.Select(x => new TweetCount { Text = x.Tag, Count = x.Count }).ToArray();
                }

                var mostLiked = _store.ConcurrentObjects.Where(x => x.data.public_metrics != null && x.data.public_metrics.like_count > 0)
                                     .OrderByDescending(x => x.data.public_metrics.like_count)
                                     .FirstOrDefault();
                if (mostLiked != null)
                {
                    res.MostLiked = new TweetCount { Text = mostLiked.data.text, Count = mostLiked.data.public_metrics.like_count };
                }

                var mostReTweeted = _store.ConcurrentObjects.Where(x => x.data.public_metrics != null && x.data.public_metrics.retweet_count > 0)
                                     .OrderByDescending(x => x.data.public_metrics.retweet_count)
                                     .FirstOrDefault();

                if (mostReTweeted != null)
                {
                    res.MostRetweeted = new TweetCount { Text = mostReTweeted.data.text, Count = mostReTweeted.data.public_metrics.retweet_count };
                }
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
    }
}
