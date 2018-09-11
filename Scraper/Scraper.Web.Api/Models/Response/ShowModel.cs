using System.Collections.Generic;

namespace Scraper.Web.Api.Models.Response
{
    public class ShowModel
    {
        public ShowModel()
        {
            Cast = new List<PersonModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PersonModel> Cast { get; set; }
    }
}
