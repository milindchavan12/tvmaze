using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Polly;
using Polly.Retry;

namespace TvMaze.DataApiClient
{
    public class JsonApiDataReader : IJsonApiDataReader
    {
        public Uri ApiBaseUrl { get; }

        public JsonApiDataReader(Uri apiBaseUrl)
        {
            ApiBaseUrl = apiBaseUrl;
        }

        public async Task<T> GetJsonData<T>(Uri relativePath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetRetryPolicy()
                .ExecuteAsync(() => new Uri(ApiBaseUrl, relativePath).ToString()
                    .ConfigureRequest(ConfigureFlurlHttpSettings)
                    .GetJsonAsync<T>(cancellationToken));
        }

        public RetryPolicy GetRetryPolicy()
	    {
		    return Policy
			    .Handle<FlurlHttpException>(ex => (int)ex.Call.Response.StatusCode == 429)
			    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)));
	    }

        public void ConfigureFlurlHttpSettings(FlurlHttpSettings settings)
	    {
		    var jsonSettings = new JsonSerializerSettings
		    {
			    NullValueHandling = NullValueHandling.Ignore,
			    DateFormatString = "yyyy-MM-dd"
		    };
		    jsonSettings.Converters.Add(new StringEnumConverter(true));
		    settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
	    }
    }
}
