using Microsoft.OpenApi.Models;
using Santander.BestStories.Infrastructure.CrossCutting;

namespace Santander.BestStories.API
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureServices();

            var app = builder.Build();

            if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseHsts();

            app.UseHealthChecks("/health-check");

            app.MapControllers();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.Run();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Santander | Hacker News API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "rodrigops.mail@gmail.com",
                        Name = "Rodrigo Pinto de Souza",
                        Url = new Uri("https://github.com/rdpnto")
                    }
                });
            });

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddHealthChecks();

            services.AddResponseCompression();

            services.AddMemoryCache();

            services.AddApplication();

            services.AddService();

            services.AddRepository();
        }
    }
}