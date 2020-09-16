using System;

namespace AlbumSearchEngine.Models
{
    [Serializable]
    public class ArtistCacheData
    {
        public string[] Albums { get; set; }

        public ArtistCacheData() { }

        public ArtistCacheData(string[] albums)
        {
            Albums = albums;
        }
    }
}
