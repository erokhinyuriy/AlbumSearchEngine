using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlbumSearchEngine.Extensions;
using AlbumSearchEngine.Models;
using iTunesSearch.Library.Models;
using iTunesSearch.Library;
using Newtonsoft.Json;


namespace AlbumSearchEngine
{
    public static class Application
    {
        public static void Start()
        {
            InitConsoleTitle();

            while (true)
            {
                var artistName = GetArtistNameFromConsole();

                var countryCode = "ru";

                var artistId = GetArtistIdByName(artistName).Result;

                iTunesSearchManager manager = new iTunesSearchManager();
                var albumResults = manager.GetAlbumsByArtistIdAsync(artistId, countryCode: countryCode);

                DisplayAlbumsInConsole(albumResults.Result.Albums);

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

        //Example http://itunes.apple.com/search?term=jack+johnson
        private static async Task<int> GetArtistIdByName(string name)
        {
            using (var client = new HttpClient())
            {
                var query = BuildQueryString(name);
                HttpResponseMessage response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStreamAsync();

                    var artistData = GetInfoFromStream<ArtistData>(responseString);

                    return artistData?.Results?.FirstOrDefault()?.ArtistId ?? 0;
                }
                else
                {
                    //Тут реализовать функционал - брать данные из кэша если нет соединения, хотя..
                    throw new NullReferenceException("Bad request");
                }
            }
        }

        private static T GetInfoFromStream<T>(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }


        private static string BuildQueryString(string name)
        {
            var newName = name.TrimStartAndEnd().Replace(" ", "+").ToLowerInvariant();
            var builder = new UriBuilder(Constants.ITUNES_SEARCH_URL) { Port = -1 };
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(builder.Query);
            query.Add(Constants.PARAMETR_TERM, newName);

            builder.Query = query.ToString();
            string queryString = builder.ToString();

            return queryString;
        }
    }
}
