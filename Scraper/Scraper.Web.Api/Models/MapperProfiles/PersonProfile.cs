using AutoMapper;
using Scraper.Core.Dto;
using Scraper.Web.Api.Models.Response;

namespace Scraper.Web.Api.Models.MapperProfiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonModel>();
        }
    }
}
