using Hephaestus.Cache.Extensions;
using Hephaestus.Cache.Configure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Cache.Contracts
{
    public class CacheBaseRepository<T> : ICacheBaseRepository<T> where T : class
    {
        private readonly IDistributedCache _cache;
        private readonly CacheConfig _cacheSettings;

        public CacheBaseRepository(IDistributedCache cache, IOptions<CacheConfig> cacheSettingsOption)
        {
            this._cache = cache;
            this._cacheSettings = cacheSettingsOption.Value;
        }

        public async Task<T> GetAsync(string key, CancellationToken token = default)
        {
            var translatedKey = key.TranslateKey<T>(_cacheSettings.Url);
            var cachedData = await _cache.GetAsync(translatedKey, token);
            if (cachedData != null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(cachedDataString);
            }

            return null;
        }

        public async Task<List<T>> GetListAsync(string key, CancellationToken token = default)
        {
            var translatedKey = key.TranslateKey<T>(_cacheSettings.Url);
            var cachedData = await _cache.GetAsync(translatedKey, token);
            if (cachedData != null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<List<T>>(cachedDataString);
            }

            return null;
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            var translatedKey = key.TranslateKey<T>(_cacheSettings.Url);
            return _cache.RefreshAsync(translatedKey, token);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            var translatedKey = key.TranslateKey<T>(_cacheSettings.Url);
            return _cache.RemoveAsync(translatedKey, token);
        }

        public Task SetAsync(string key, T value, CancellationToken token = default)
        {
            var translatedKey = key.TranslateKey<T>(_cacheSettings.Url);
            string cachedDataString = JsonSerializer.Serialize(value);
            var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(this._cacheSettings.AbsoluteExpiration))
                .SetSlidingExpiration(TimeSpan.FromMinutes(this._cacheSettings.SlidingExpiration));

            return _cache.SetAsync(translatedKey, dataToCache, options, token);
        }
    }
}
