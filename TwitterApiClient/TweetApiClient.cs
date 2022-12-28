using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using TwitterApiClient.Models;

namespace TwitterApiClient
{
    public class TweetApiClient : ITweetApiClient
    {
        private HttpClient _httpClientInstance;
        public HttpClient HttpClient => _httpClientInstance;
        public TweetApiClient()
        {
            //GetAuthorizedClient(context);
        }

        private void GetAuthorizedClient(HttpContext context)
        {
            HttpClientHandler handler = new();
            handler.UseDefaultCredentials = true;
            // Create an HttpClient object
            //var telemetryHandler = new TelemetryHttpHandler(handler);
            _httpClientInstance = new HttpClient();
            if (context != null)
            {
                var requestHeaders = context.Request.Headers;//HttpContext.Current.Request.Headers;

                if (requestHeaders.Keys.Contains("Authorization"))
                {
                    string header = requestHeaders["Authorization"];
                    string token = Regex.Replace(header, "Bearer ", "", RegexOptions.IgnoreCase);

                    if (!string.IsNullOrEmpty(token))
                        _httpClientInstance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    if (_httpClientInstance.DefaultRequestHeaders.Accept.Count(x => x.MediaType == "application/json") == 0)
                        _httpClientInstance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
            }

        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, Uri requestUri, HttpContent payload)
        {
            var request = new HttpRequestMessage(httpMethod, requestUri) { Content = payload };

            return await _httpClientInstance.SendAsync(request).ConfigureAwait(false);
        }

        private void setToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
                _httpClientInstance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (_httpClientInstance.DefaultRequestHeaders.Accept.Count(x => x.MediaType == "application/json") == 0)
                _httpClientInstance.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public List<Tweet> FetchTweets(string token)
        {
            setToken(token);
            //invoke twitter api and fetch result
            return new List<Tweet>();
        }


        //public async Task<IEnumerable<Rootobject>> GetTwitts(string accessToken)
        //{
        //    //if (accessToken == null)
        //    IEnumerable<Rootobject> result = new List<Rootobject>();

        //    //{
        //    //    accessToken = await GetAccessToken();
        //    //}
        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    if (httpClient.DefaultRequestHeaders.Accept.Count(x => x.MediaType == "application/json") == 0)
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var stream = await httpClient.GetStreamAsync("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=created_at,entities");

        //    using (var reader = new StreamReader(stream))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            //We are ready to read the stream
        //            result = reader.ToEnumerableRoot();
        //        }
        //    }
        //    return result;
        //}




        //public async Task<string> GetAccessToken()
        //{
        //    var httpClient = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
        //    var customerInfo = Convert.ToBase64String(new UTF8Encoding().GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
        //    request.Headers.Add("Authorization", "Basic " + customerInfo);
        //    request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        //    HttpResponseMessage response = await httpClient.SendAsync(request);

        //    string json = await response.Content.ReadAsStringAsync();
        //    var serializer = new JavaScriptSerializer();
        //    dynamic item = serializer.Deserialize<object>(json);
        //    return item["access_token"];
        //}
    }




}