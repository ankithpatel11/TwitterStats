using MediatR;
using TwitterStatistics.Shared.Data.Models;

namespace TwitterStatistics.Api.Queries
{
    public class GetTwitterStatsQuery : IRequest<TweetStat>
    {
    }
}
