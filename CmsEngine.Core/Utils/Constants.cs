namespace CmsEngine.Core
{
    public static class Constants
    {
        public const string SiteUrl = "[site_url]";
        public const string Culture = "[culture]";
        public const string ShortCulture = "[short_culture]";
        public const string Type = "[type]";
        public const string Slug = "[slug]";

        public const string WwwDot = "www.";
        public const string Localhost = "localhost";

        public static class ImagePath
        {
            public const string Default = "/image/{0}/{1}";
            public const string Path640 = "/image/{0}/640x426_{1}";
            public const string Path320 = "/image/{0}/320x213_{1}";
            public const string Path120 = "/image/{0}/120x120_{1}";
        }

        public static class CacheKey
        {
            public const string Instance = "Instance";
        }
    }
}
