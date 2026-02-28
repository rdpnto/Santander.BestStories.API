using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Santander.BestStories.Application.Interfaces;
using Santander.BestStories.Application.UseCases;
using Santander.BestStories.Domain.Interfaces.Repositories;
using Santander.BestStories.Domain.Interfaces.Services;
using Santander.BestStories.Domain.Services;
using Santander.BestStories.Infrastructure.Repositories;

namespace Santander.BestStories.Infrastructure.CrossCutting
{
    public static class IoC
    {
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync
                (
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timespan.Seconds} seconds");
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync
                (
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit broken for {breakDelay.TotalSeconds} seconds. Error: {outcome.Exception?.Message}");
                    },
                    onReset: () => Console.WriteLine("Circuit reset")
                );
        }

        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBestStoriesUseCase, BestStoriesUseCase>();
        }

        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IHackerNewsService, HackerNewsService>();
        }

        public static void AddRepository(this IServiceCollection services)
        {
            services
                .AddHttpClient<IHackerNewsRepository, HackerNewsRepository>(client =>
                {
                    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }
    }
}
