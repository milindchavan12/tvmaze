using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMaze.Persistence.Models;
using TvMaze.ApiService.Configuration;

namespace TvMaze.ApiService
{
    public class Startup
    {
        private const string TvMazeApiTitle = "TV Maze API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvc();
            services.ConfigureSwagger(TvMazeApiTitle);
            services.ConfigureShowContext(Configuration);
            services.ConfigureDependencyInjection();
            services.ConfigureLogging(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", TvMazeApiTitle));

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ShowContext>().Database.EnsureCreated();
            }
            app.UseMvc();
        }
    }
}
