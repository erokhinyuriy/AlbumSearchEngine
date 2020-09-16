using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AlbumSearchEngine.Interfaces;
using AlbumSearchEngine.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AlbumSearchEngine
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider;

        private static async Task Main(string[] args)
        {
            RegisterServices();

            var itunesService = _serviceProvider.GetService<ITunesRepository>();

            var app = new Application(itunesService);
            await app.Start();

            await DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddScoped<ITunesRepository, iTunesRepository>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        private static async Task DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IAsyncDisposable)
            {
                await ((IAsyncDisposable)_serviceProvider).DisposeAsync();
            }
        }
    }
}
