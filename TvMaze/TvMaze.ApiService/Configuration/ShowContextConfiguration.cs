using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMaze.Persistence.Models;

namespace TvMaze.ApiService.Configuration
{
    internal static class ShowContextConfiguration
    {
        public static void ConfigureShowContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShowContext>(options => options.ConfigureShowContextOptions(configuration));
        }

        public static void ConfigureShowContextOptions(this DbContextOptionsBuilder options, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                options.UseInMemoryDatabase(databaseName: "tvmaze");
            }
            else
            {
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly("TvMaze.Persistence"));
            }
        }
    }
}
