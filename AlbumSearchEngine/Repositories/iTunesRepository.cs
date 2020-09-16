using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlbumSearchEngine.Extensions;
using AlbumSearchEngine.Interfaces;
using AlbumSearchEngine.Models;
using iTunesSearch.Library;
using iTunesSearch.Library.Models;
using Newtonsoft.Json;

namespace AlbumSearchEngine.Repositories
{
    public class iTunesRepository : ITunesRepository
    {
        private readonly iTunesSearchManager _manager;

        public iTunesRepository()
        {
            _manager = new iTunesSearchManager();
        }

        public async Task<List<Album>> GetAlbumsByName(string nameArtist)
        {
            var artistId = await GetArtistIdByName(nameArtist);
            var albumResults = await _manager.GetAlbumsByArtistIdAsync(artistId, countryCode: Constants.CODE_RU);
            return albumResults.Albums;
        }

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
