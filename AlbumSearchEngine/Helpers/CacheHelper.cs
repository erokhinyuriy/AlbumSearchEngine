using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AlbumSearchEngine.Helpers
{
    public class CacheHelper
    {
        private static readonly BinaryFormatter _formatter = new BinaryFormatter();

        public static void Set<T>(string key, T obj) where T : class
        {
            using Stream fs = new FileStream($"{key}.dat", FileMode.OpenOrCreate);
            _formatter.Serialize(fs, obj);
        }

        public static T Get<T>(string key) where T : class
        {
            var fileName = $"{key}.dat";

            if (File.Exists(fileName))
            {
                using FileStream fs = new FileStream(fileName, FileMode.Open);
                T obj = (T)_formatter.Deserialize(fs);
                return obj;
            }

            throw new FileNotFoundException("Cache file is not found!");
        }
    }
}
