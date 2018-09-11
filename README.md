# scraper
Web scraping application targeting TvMaze api and scraping show and cast information.

## Scraper.Terminal
is used for scraping the api and extracting the data. In current version MongoDB is used for persistence. By default it is using localhost instance ("mongodb://localhost"). Due to the rate limit scraping process really takes long so cancellation support is provided.

Show data is persisted in two steps: firstly raw json representation of a show and cast is saved in one collection (*showsRaw*), then currently required data is extracted to the dedicated collection (*shows*).

## Scraper.Web.Api
is the web api for providing the access to the scraped data in required format.

There's currently only one endpoint:
/api/shows?pagesize=10&pagenumber=5

Both pagesize and pagenumber parameters are optional (default values are 10 and 0 respectively).

**Swagger** is awailable at
/swagger
