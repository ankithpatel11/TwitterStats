using MediatR;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TwitterStatistics.Shared.Data.Helpers;
using TwitterStatistics.Shared.Data.Models;

namespace TwitterStatistics.Api.Site.HostedService
{
    public class TwitterHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer? _timer = null;
        private IMediator _mediator;
        private Store _response;

        public TwitterHostedService(IMediator mediator, Store response)
        {
            _mediator = mediator;
            _response = response;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var twitterPullTimer = ApplicationSettings.GetAppSettings("TwitterPullTimer");
            var timerForPull = (string.IsNullOrEmpty(twitterPullTimer)) ? 3000 : Convert.ToInt32(twitterPullTimer);
            _timer = new Timer(DoWork, null, timerForPull, Timeout.Infinite);
            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            if (_response.ConcurrentObjects == null)
            {
                _response.ConcurrentObjects = new System.Collections.Concurrent.ConcurrentBag<Rootobject>();
            }
            GetTwittsV2(ApplicationSettings.GetAppSettings("TwitterBearerToken"));
        }

        public async Task GetTwittsV2(string accessToken)
        {
            HttpClient httpClient = new HttpClient();//will need to inject this dependency
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (httpClient.DefaultRequestHeaders.Accept.Count(x => x.MediaType == "application/json") == 0)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            var url = ApplicationSettings.GetAppSettings("TwitterURL");
            var stream = await httpClient.GetStreamAsync(url);

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    //We are ready to read the stream
                    string s = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        Rootobject result = JsonConvert.DeserializeObject<Rootobject>(s!)!;
                        if (result != null)
                        {
                            _response.ConcurrentObjects.Add(result);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
