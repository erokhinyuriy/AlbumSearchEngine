using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbumSearchEngine.Interfaces;
using iTunesSearch.Library.Models;

namespace AlbumSearchEngine
{
    public class Application
    {
        private readonly ITunesRepository _repository;

        public Application(ITunesRepository repository)
        {
            _repository = repository;
        }

        public async Task Start()
        {
            InitConsoleTitle();

            while (true)
            {
                var artistName = GetArtistNameFromConsole();
                var albums = await _repository.GetAlbumsByName(artistName);

                DisplayAlbumsInConsole(albums);

                ClearConsole();
            }
        }

        #region Console actions

        /// <summary>
        /// Инициализация имени приложения
        /// </summary>
        private static void InitConsoleTitle()
        {
            Console.Title = Constants.APPLICATION_NAME;
        }

        /// <summary>
        /// Пользователь может выбрать - очистить консоль или нет
        /// </summary>
        private static void ClearConsole()
        {
            Console.WriteLine();
            Console.Write(Constants.CLEAR_CONSOLE);
            var result = Console.ReadLine()?.ToLower();

            switch (result)
            {
                case Constants.YES:
                    Console.Clear();
                    break;
                case Constants.NO:
                    Console.WriteLine();
                    Console.WriteLine(Constants.SEPARATOR);
                    Console.WriteLine();
                    break;
                default:
                    Console.WriteLine(Constants.SEPARATOR);
                    Console.WriteLine(Constants.ACTION_IS_NOT_DEFINED);
                    Console.WriteLine(Constants.SEPARATOR);
                    break;
            }
        }

        /// <summary>
        /// Получает имя исполнителя, введенного пользователем в консоле
        /// </summary>
        /// <returns></returns>
        private static string GetArtistNameFromConsole()
        {
            Console.Write(Constants.ENTER_ARTIST);
            var result = Console.ReadLine();
            return result;
        }

        /// <summary>
        /// Выводит все альбомы на консоль
        /// </summary>
        /// <param name="albums"></param>
        private static void DisplayAlbumsInConsole(List<Album> albums)
        {
            Console.WriteLine();
            int i = 1;

            foreach (var album in albums)
            {
                if (!string.IsNullOrWhiteSpace(album.CollectionName))
                {
                    Console.WriteLine($">>> {i}. {album.CollectionName}");
                    i++;
                }
            }
        }

        #endregion

    }
}
