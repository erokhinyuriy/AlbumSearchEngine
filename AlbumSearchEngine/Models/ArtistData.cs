using System.Collections.Generic;

namespace AlbumSearchEngine.Models
{
    public class ArtistData
    {
        public int ResultCount { get; set; }
        public List<Result> Results { get; set; }
    }
}
