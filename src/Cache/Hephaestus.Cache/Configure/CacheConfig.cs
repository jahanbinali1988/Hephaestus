namespace Hephaestus.Cache.Configure
{
    public class CacheConfig
    {
        public string Url { get; set; }
        public int AbsoluteExpiration { get; set; }
        public int SlidingExpiration { get; set; }
    }
}
