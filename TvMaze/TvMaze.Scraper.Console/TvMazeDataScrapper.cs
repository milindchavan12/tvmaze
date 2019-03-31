using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TvMaze.Persistence.Interfaces;
using TvMaze.Persistence.Models;
using TvMaze.Domains.DTO;
using TvMaze.DataApiClient;

namespace TvMaze.Scraper.Console
{
    public class TvMazeDataScrapper
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<TvMazeDataScrapper> _logger;
        private readonly TvMazeService _tvMazeService;

        public TvMazeDataScrapper(IConfigurationRoot configuration, ILogger<TvMazeDataScrapper> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _tvMazeService = GetTvMazeService();
        }

        private TvMazeService GetTvMazeService()
        {
            var baseurl = new Uri(_configuration.GetSection("Config")["TvMazeServiceUrl"]);

            var jsonJsonApiDataReader = new JsonApiDataReader(baseurl);

            return new TvMazeService(jsonJsonApiDataReader, GetTvMazeServiceLogger());
        }

        public async Task<List<Show>> GetShows(IShowRepository showRepository, int pageNumber)
        {
            var shows = new List<Show>();
            _logger.LogInformation($"Stating copying data from {nameof(pageNumber)} {pageNumber}");

            var showDtos = await _tvMazeService.GetShowsByPage(pageNumber).ConfigureAwait(false);
            _logger.LogInformation($"Found {showDtos.Count} Shows from TvMaze API service");

            foreach (var showDto in showDtos)
            {
                var tvMazeShowWithCast = await _tvMazeService.GetShowDataById(showDto.Id).ConfigureAwait(false);

                _logger.LogInformation($"Found TvMaze show {tvMazeShowWithCast} with cast");

                var show = new Show {Id = tvMazeShowWithCast.Id, Name = tvMazeShowWithCast.Name};

                show.Cast.AddRange(tvMazeShowWithCast.Cast
                    .TakeWhile(personDto => personDto != null)
                    .Distinct(new PersonDtoEqualityComparer())
                    .Select(personDto => new Cast
                    {
                        CastPersonId = personDto.Id,
                        ShowId = show.Id,
                        Person = GetPerson(showRepository, personDto)
                    }));

                shows.Add(show);
            }
            return shows;
        }

        private static Person GetPerson(IShowRepository showRepository, PersonDto personDto)
        {
            if (showRepository.HasPersonWithId(personDto.Id))
            {
                return null;
            }

            return new Person
            {
                Id = personDto.Id,
                Name = personDto.Name,
                Birthdate = personDto.Birthdate
            };
        }

        private static ILogger<TvMazeService> GetTvMazeServiceLogger()
        {
            var loggerFactory = new LoggerFactory().AddConsole();
            var logger = loggerFactory.CreateLogger<TvMazeService>();
            return logger;
        }
    }
}