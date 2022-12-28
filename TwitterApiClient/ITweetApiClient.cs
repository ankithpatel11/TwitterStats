using TwitterApiClient.Models;

namespace TwitterApiClient
{
    public interface ITweetApiClient
    {
        public List<Tweet> FetchTweets(string token);

        //Task<IEnumerable<Rootobject>> GetTwitts(string accessToken);
    }
}