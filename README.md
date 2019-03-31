Create an application that:

- Scrapes the TVMaze API for show and cast information;
- Persists the data in storage;
- Provides the scraped data using a REST API.

The Application Consists of following components:

1. DataApiClient : Fetches the Data from TvMze Api and Provides the Domain object for processing.
2. Persistance : Persists the Scraped data in SQL Server Database.
3. Scraper Console : Console Application scraps the data by DataApiClient and Stores in database.
4. ApiService : Provides the scraped data using a REST API 

## How to execute
1. Run TvMaze.Scraper.Console and scrape the data from http://www.tvmaze.com/api
2. Run TvMaze.ApiService for REST API 
3. Swagger API : /swagger/index.html for testing API on any browser