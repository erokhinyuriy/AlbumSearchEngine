namespace AlbumSearchEngine
{
    public static class Constants
    {
        public const string APPLICATION_NAME = "Album Search Engine";

        #region Console strings and alerts

        public const string ENTER_ARTIST = "Введите исполнителя: ";
        public const string CLEAR_CONSOLE = "Очистить консоль? y/n: ";
        public const string ACTION_IS_NOT_DEFINED = "Action is not defined!";

        #endregion

        #region ITunes parameters

        public const string PARAMETR_TERM = "term";
        public const string ITUNES_SEARCH_URL = @"http://itunes.apple.com/search";

        #endregion

        #region Country codes

        public const string CODE_RU = "RU";

        #endregion

        #region Secret code

        public const ushort CRYPTO_SECRET_CODE = 0x00952;

        #endregion

        #region Other

        public const string YES = "y";
        public const string NO = "n";

        public const string SEPARATOR = "------------------------------------------------------";

        #endregion
    }
}
