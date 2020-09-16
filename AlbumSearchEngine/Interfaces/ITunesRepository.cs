using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using iTunesSearch.Library.Models;

namespace AlbumSearchEngine.Interfaces
{
    public interface ITunesRepository
    {
        Task<List<Album>> GetAlbumsByName(string nameArtist);
    }
}
