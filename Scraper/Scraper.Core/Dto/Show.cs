using System.Collections.Generic;

namespace Scraper.Core.Dto
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Person> Cast { get; set; }
    }
}
