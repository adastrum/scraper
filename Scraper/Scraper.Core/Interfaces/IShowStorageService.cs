using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scraper.Core.Dto;

namespace Scraper.Core.Interfaces
{
    public interface IShowStorageService
    {
        Task SaveRawDataAsync(string showJson, string castJson);
        Task ExtractDataAsync(CancellationToken token);
        Task<IEnumerable<Show>> GetShowsAsync(int pageSize, int pageNumber);
    }
}
