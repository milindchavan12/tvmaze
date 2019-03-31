using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TvMaze.DataApiClient;

namespace TvMaze.Tests.TvMazeApiClient
{
    [TestClass]
    public class IntegrationTests
    {
        private ITvMazeService _tvMazeService;

        [TestInitialize]
        public void Initialize()
        {
            var jsonJsonApiDataReader = new JsonApiDataReader(new Uri("http://api.tvmaze.com"));

            _tvMazeService = new TvMazeService(jsonJsonApiDataReader, GetLogger());
        }

        [TestMethod]
        public void GetShowsByPage_WherePageNumberEquals1_ShouldReturnDataFromTvMazeApi()
        {
            var showData = _tvMazeService.GetShowsByPage(1);

            showData.Wait();

            var showDtos = showData.Result;

            Assert.IsTrue(showDtos.Count > 1);
            Assert.IsTrue(showDtos.All(showDto => !string.IsNullOrEmpty(showDto.Name)));
            Assert.IsTrue(showDtos.All(showDto => showDto.Id >= 0));
            Assert.IsTrue(showDtos.All(showDto => showDto.Cast.Count == 0));
        }

        [TestMethod]
        public void GetShowDataById_WhereShowIdEquals1_ShouldReturnDataFromTvMazeApi()
        {
            var showData = _tvMazeService.GetShowDataById(1);

            showData.Wait();

            var showDto = showData.Result;

            Assert.IsTrue(showDto.Id == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(showDto.Name));
            Assert.IsTrue(showDto.Cast.Count > 0);
        }

        private static ILogger<TvMazeService> GetLogger()
        {
            var loggerFactory = new LoggerFactory().AddConsole();
            var logger = loggerFactory.CreateLogger<TvMazeService>();
            return logger;
        }
    }
}
