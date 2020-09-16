using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AlbumSearchEngine.Exceptions;
using AlbumSearchEngine.Extensions;
using AlbumSearchEngine.Helpers;
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

        public async Task<List<Album>> GetAlbumsByName(string artistName)
        {
            var newArtistName = artistName.TrimStartAndEnd().Replace(" ", "+").ToLowerInvariant();
            var cryptoArtistName = CryptoStringHelper.GetEncodeString(newArtistName);

            try
            {
                var artistId = await GetArtistIdByName(newArtistName);
                var albumResults = await _manager.GetAlbumsByArtistIdAsync(artistId, countryCode: Constants.CODE_RU);

                var artistCacheData = new ArtistCacheData(albumResults.Albums.Select(x => x.CollectionName).ToArray());
                CacheHelper.Set(cryptoArtistName, artistCacheData.Albums);

                return albumResults.Albums;
            }
            catch (HttpErrorStatusCodeException ex)
            {
                var cacheData = CacheHelper.Get<string[]>(cryptoArtistName);
                return cacheData.Select(x => new Album
                {
                    CollectionName = x
                }).ToList();
            }
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

                    return artistData.Results.FirstOrDefault()?.ArtistId ?? 0;
                }

                throw new HttpErrorStatusCodeException("The request is not valid or the server is unavailable.");
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
            var builder = new UriBuilder(Constants.ITUNES_SEARCH_URL) { Port = -1 };
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(builder.Query);
            query.Add(Constants.PARAMETR_TERM, name);

            builder.Query = query.ToString();
            string queryString = builder.ToString();

            return queryString;
        }
    }
}
