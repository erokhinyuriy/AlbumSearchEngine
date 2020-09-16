using System.Collections.Generic;
using System.Threading.Tasks;
using iTunesSearch.Library.Models;

namespace AlbumSearchEngine.Interfaces
{
    public interface ITunesRepository
    {
        Task<List<Album>> GetAlbumsByName(string nameArtist);
    }
}
