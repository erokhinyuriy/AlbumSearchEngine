using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlbumSearchEngine.Helpers
{
    public class CryptoStringHelper
    {
        public static string GetEncodeString(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            string enText = Convert.ToBase64String(bytes);
            return enText;
        }
    }
}
