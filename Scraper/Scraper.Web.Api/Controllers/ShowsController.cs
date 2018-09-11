using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ShowsController(
            IShowStorageService showStorageService,
            IMapper mapper
        )
        {
            _showStorageService = showStorageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ShowModel>> GetShowsAsync([FromQuery]ShowFilter filter)
        {
            var shows = await _showStorageService.GetShowsAsync(filter.PageSize, filter.PageNumber);

            return shows.Select(x => _mapper.Map<ShowModel>(x)).ToList();
        }
    }
}
