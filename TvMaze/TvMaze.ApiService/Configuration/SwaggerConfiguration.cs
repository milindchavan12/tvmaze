using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace TvMaze.ApiService.Configuration
{
    internal static class SwaggerConfiguration
    {
        public static void ConfigureSwagger(this IServiceCollection services, string title)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = title, Version = "v1" });
                options.DescribeAllParametersInCamelCase();
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }
    }
}
