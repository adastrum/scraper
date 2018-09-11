using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scraper.Core.Interfaces;
using Scraper.Web.Api.Models.Request;
using Scraper.Web.Api.Models.Response;

namespace Scraper.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class ShowsController : Controller
    {
        private readonly IShowStorageService _showStorageService;

        public ShowsController(IShowStorageService showStorageService)
        {
            _showStorageService = showStorageService;
        }

        [HttpGet]
        public async Task<IEnumerable<ShowModel>> GetShowsAsync([FromQuery]ShowFilter filter)
        {
            var shows = await _showStorageService.GetShowsAsync(filter.PageSize, filter.PageNumber);

            return shows.Select(x => new ShowModel
            {
                Id = x.Id,
                Name = x.Name,
                Cast = x.Cast.Select(y => new PersonModel { Id = y.Id, Name = y.Name, Birthday = y.Birthday }).OrderByDescending(z => z.Birthday)
            });
        }
    }
}
