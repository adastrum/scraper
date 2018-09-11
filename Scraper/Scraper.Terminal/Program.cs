using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Scraper.Core.Interfaces;
using Scraper.Data.MongoDb;
using Scraper.Scraping;

namespace Scraper.Terminal
{
    internal class Program
    {
        //todo: async main
        private static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IApiScrapingService, ApiScrapingService>()
                .AddSingleton<IShowStorageService, ShowStorageService>()
                .BuildServiceProvider();

            var apiScrapingService = serviceProvider.GetService<IApiScrapingService>();
            var showStorageService = serviceProvider.GetService<IShowStorageService>();

            var cts = new CancellationTokenSource();

            apiScrapingService.ScrapeApiAsync(cts.Token);
            WaitOrCancel("scraping the API...", cts);

            var cts2 = new CancellationTokenSource();

            //showStorageService.ExtractDataAsync(cts2.Token);
            //WaitOrCancel("extracting data...", cts2);

            showStorageService.ExtractDataAsync(cts2.Token);
        }

        private static void WaitOrCancel(string message, CancellationTokenSource cts)
        {
            Console.WriteLine(message);
            Console.WriteLine("press ESC to terminate");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    cts.Cancel();

                    break;
                }
            }
        }
    }
}
