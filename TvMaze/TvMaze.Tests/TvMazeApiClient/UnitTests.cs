using System;
using System.Linq;
using System.Net;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TvMaze.DataApiClient;

namespace TvMaze.Tests.TvMazeApiClient
{
    [TestClass]
    public class UnitTests
    {
        private readonly JsonApiDataReader _jsonJsonApiDataReader =
            new JsonApiDataReader(new Uri("http://api.some.tvmaze.api.url"));

        [TestMethod]
        public void GetShowDataById_WhenApiServiceReturns401_ShouldRecieveUnauthorizedFlurlHttpException()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(null, 401);

                var tvMazeService = new TvMazeService(_jsonJsonApiDataReader, GetLogger());

                var showData = tvMazeService.GetShowDataById(1);

                try
                {
                    showData.Wait();
                }
                catch (AggregateException aggregateException)
                {
                    aggregateException.Handle(exception =>
                    {
                        Assert.IsTrue(exception is FlurlHttpException flurlHttpException &&
                                      flurlHttpException.Call.HttpStatus == HttpStatusCode.Unauthorized);
                        return true;
                    });
                }
            }
        }

        [TestMethod]
        public void GetShowDataById_WhereShowIdEqualsARandomId_ExpectsReturnDataToMatchId()
        {
            var randomId = new Random().Next(1, 10000);

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(new JObject { { "id", randomId } });

                var sut = new TvMazeService(_jsonJsonApiDataReader, GetLogger());

                var showData = sut.GetShowDataById(randomId);

                showData.Wait();

                Assert.IsTrue(showData.Result.Id == randomId);
            }
        }

        [TestMethod]
        public void GetShowsByPage_WherePageNumberEqualsARandomPage_ExpectsReturnDataToMatch()
        {
            var randomPageId = new Random().Next(1, 10000);
            var randomId = new Random().Next(1, 10000);

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(GetTestData(randomId));

                var sut = new TvMazeService(_jsonJsonApiDataReader, GetLogger());

                var showData = sut.GetShowsByPage(randomPageId);

                showData.Wait();

                Assert.IsTrue(showData.Result.Count == 1);
                Assert.IsTrue(showData.Result.First().Id == randomId);
                Assert.IsTrue(showData.Result.First().Name == $"some show {randomId}");
            }
        }

        private static JArray GetTestData(int randomId)
        {
            JObject[] data =
            {
                new JObject(
                    new JProperty("id", randomId),
                    new JProperty("name", $"some show {randomId}"))
            };
            var testData = new JArray();
            foreach (var jObject in data)
            {
                testData.Add(jObject);
            }
            return testData;
        }

        private static ILogger<TvMazeService> GetLogger()
        {
            var loggerFactory = new LoggerFactory().AddConsole();
            var logger = loggerFactory.CreateLogger<TvMazeService>();
            return logger;
        }
    }

}
