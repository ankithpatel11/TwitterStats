using MediatR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using TwitterStatistics.Shared.Data.Helpers;
using TwitterStatistics.Shared.Data.Models;
using TwitterStatistics.Tms.Commands;

namespace TwitterStatistics.Tms.CommandHandlers
{
    public class StoreTweetsCommandHandler : IRequestHandler<StoreTweetsCommand, bool>
    {
        private Store _response;
        public StoreTweetsCommandHandler(Store response)
        {
            _response = response;
        }

        public async Task<bool> Handle(StoreTweetsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_response.ConcurrentObjects == null)
                {
                    _response.ConcurrentObjects = new System.Collections.Concurrent.ConcurrentBag<Rootobject>();
                }
                await GetTwittsV2(request.BearerToken);

                //store the tweet data obtained to database if required

                return true;
            }
            catch (Exception ex)
            {
                Debugger.Log(1, "Error", ex.ToString());
            }
            return false;
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
    }
}


