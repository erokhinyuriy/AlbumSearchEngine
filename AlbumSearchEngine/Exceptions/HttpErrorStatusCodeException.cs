using System;
using System.Collections.Generic;
using System.Text;

namespace AlbumSearchEngine.Exceptions
{
    public class HttpErrorStatusCodeException : Exception
    {
        public HttpErrorStatusCodeException(string message) : base(message)
        { }
    }
}
