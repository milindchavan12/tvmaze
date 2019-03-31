using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMaze.Persistence.Interfaces;
using TvMaze.Persistence.Models;
using TvMaze.Persistence.Repositories;

namespace TvMaze.Scraper.Console
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            System.Console.WriteLine("TvMaze Show Scraper Console");

            var serviceCollection = new ServiceCollection();
            var (configuration, showRepository) = ConfigureServices(serviceCollection);

            var tvMazeDataScrapper = new TvMazeDataScrapper(configuration, GetTvMazeDataScrapperLogger());

            var pageNumber = await GetPageNumber(showRepository);

            while (true)
            {
                var shows = await tvMazeDataScrapper.GetShows(showRepository, pageNumber);

                if (shows.Count == 0) break;

                foreach (var show in shows)
                {
                    await showRepository.AddShow(show);
                }

                pageNumber++;
            }
        }

        private static async Task<int> GetPageNumber(IShowRepository showRepository)
        {
            var startId = await showRepository.GetLastShowId();
            var pageNumber = (int)Math.Floor((double)startId / 250);
            return pageNumber == 0 ? 1 : pageNumber;
        }

        private static (IConfigurationRoot, IShowRepository) ConfigureServices(ServiceCollection serviceCollection)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            var connectionStr = configuration.GetConnectionString("DefaultConnection");

            serviceCollection
                .AddDbContext<ShowContext>(options => options.UseSqlServer(connectionStr).EnableSensitiveDataLogging())
                .AddTransient<IShowRepository, ShowRepository>()
                .AddTransient<IShowContext, ShowContext>()
                .AddSingleton(new LoggerFactory()
                    .AddConsole(configuration.GetSection("Logging"))
                    .AddDebug())
                .AddLogging();

            var services = serviceCollection.BuildServiceProvider();

            var showContext = services.GetService<ShowContext>();
            showContext.Database.EnsureCreated();

            return (configuration, services.GetService<IShowRepository>());
        }

        private static ILogger<TvMazeDataScrapper> GetTvMazeDataScrapperLogger()
        {
            var loggerFactory = new LoggerFactory().AddConsole();
            return loggerFactory.CreateLogger<TvMazeDataScrapper>();
        }
    }
}
