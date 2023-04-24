using System;

namespace Hephaestus.Cache.Extensions
{
    public static class KeyExtensions
    {
        public static string TranslateKey<T>(this string key, string url) where T : class
        {
            return $"{typeof(T).Name}-{url}-{key}";
        }
    }
}
