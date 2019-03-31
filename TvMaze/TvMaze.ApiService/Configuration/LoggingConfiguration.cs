using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TvMaze.ApiService.Configuration
{
    internal static class LoggingConfiguration
    {
        public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            });
        }
    }
}
