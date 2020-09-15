namespace AlbumSearchEngine.Extensions
{
    public static class StringExtension
    {
        public static string TrimStartAndEnd(this string value)
        {
            return value.TrimStart().TrimEnd();
        }
    }
}
