namespace Scraper.Web.Api.Models.Request
{
    public class ShowFilter
    {
        public ShowFilter()
        {
            PageSize = 10;
            PageNumber = 0;
        }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
