using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Scraper.Core.Interfaces;

namespace Scraper.Scraping
{
    public class ApiScrapingService : IApiScrapingService
    {
        //todo: move to config
        private const int BatchSize = 100;
        private const int MaxTryCount = 5;
        private const int RetryDelay = 1000;
        private const string TvMazeApiAddress = "http://api.tvmaze.com";

        private readonly IShowStorageService _showStorageService;
        private readonly HttpClient _client;

        public ApiScrapingService(IShowStorageService showStorageService)
        {
            _showStorageService = showStorageService;

            _client = new HttpClient();
        }

        public async Task ScrapeApiAsync(CancellationToken token)
        {
            var lastId = 1;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var tasks = Enumerable.Range(lastId, BatchSize)
                    .Select(showId => ScrapeApiAsync(showId, MaxTryCount, RetryDelay));

                var results = await Task.WhenAll(tasks);
                if (results.All(x => !x))
                {
                    break;
                }

                lastId += BatchSize;
            }
        }

        private async Task<bool> ScrapeApiAsync(int showId, int maxTryCount, int retryDelay)
        {
            var tryCount = 0;

            do
            {
                try
                {
                    var showJson = await GetShowJson(showId);

                    var castJson = await GetCastJson(showId);

                    await _showStorageService.SaveRawDataAsync(showJson, castJson);

                    return true;
                }
                catch (HttpRequestException)
                {
                    tryCount++;

                    await Task.Delay(retryDelay);
                }
            } while (tryCount < maxTryCount);

            return false;
        }

        private async Task<string> GetShowJson(int showId)
        {
            var showUri = new Uri(new Uri(TvMazeApiAddress), $"shows/{showId}");

            return await _client.GetStringAsync(showUri);
        }

        private async Task<string> GetCastJson(int showId)
        {
            var castUri = new Uri(new Uri(TvMazeApiAddress), $"shows/{showId}/cast");

            return await _client.GetStringAsync(castUri);
        }
    }
}
