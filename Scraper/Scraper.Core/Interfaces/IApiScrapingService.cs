using System.Threading;
using System.Threading.Tasks;

namespace Scraper.Core.Interfaces
{
    public interface IApiScrapingService
    {
        Task ScrapeApiAsync(CancellationToken token);
    }
}
