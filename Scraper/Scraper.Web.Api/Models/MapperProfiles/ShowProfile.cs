using System.Linq;
using AutoMapper;
using Scraper.Core.Dto;
using Scraper.Web.Api.Models.Response;

namespace Scraper.Web.Api.Models.MapperProfiles
{
    public class ShowProfile : Profile
    {
        public ShowProfile()
        {
            CreateMap<Show, ShowModel>()
                .ForMember(
                    x => x.Cast,
                    x => x.ResolveUsing(
                        show => show.Cast.OrderByDescending(person => person.Birthday)));
        }
    }
}
