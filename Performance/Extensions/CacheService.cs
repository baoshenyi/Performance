using log4net.Core;
using System;
using System.Runtime.Caching;

namespace MajorProjects.Data
{
    public interface ICacheService
    {
        T Get<T>(string key, Func<T> ifNotSetAddThis);
        T Get<T>(string key, Func<T> ifNotSetAddThis, int cacheInMinutes);
        void Invalidate(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly ILogger _logger;

        public CacheService(ILogger logger)
        {
            _logger = logger;
        }

        private static readonly object gate = new object();

        public T Get<T>(string key, Func<T> ifNotSetAddThis)
        {
            return Get(key, ifNotSetAddThis, 240);
        }

        public T Get<T>(string key, Func<T> ifNotSetAddThis, int cacheInMinutes)
        {
            var cache = MemoryCache.Default;
            var result = cache[key];

            if (result != null)
            {
                //_logger.LogDebugMessage(this.GetType(), "Loading {0} from cache without lock", key);
                return (T)result;
            }
            else
            {
                lock (gate)
                {
                    //_logger.LogDebugMessage(this.GetType(), "Loading {0} from cache", key);
                    cache = MemoryCache.Default;
                    result = cache[key];
                    if (result == null)
                    {
                        //_logger.LogDebugMessage(this.GetType(), "{0} was not found in cache", key);
                        result = ifNotSetAddThis();
                        var policyLastUpdate = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheInMinutes) };
                        CacheByKey<T>(key, cache, result, policyLastUpdate);
                        //_logger.LogDebugMessage(this.GetType(), "{0} added to cache and will expire at {1}", key, policyLastUpdate.AbsoluteExpiration);
                    }
                    else
                    {
                        //_logger.LogDebugMessage(this.GetType(), "{0} was found in cache. Returning cached data", key);
                    }
                    return (T)result;
                }
            }
        }

        private static void CacheByKey<T>(string key, MemoryCache cache, object result, CacheItemPolicy policyLastUpdate)
        {
            cache.Add(new CacheItem(key, result), policyLastUpdate);
        }

        public void Invalidate(string key)
        {
            //_logger.LogDebugMessage(this.GetType(), "Removing {0} from cache", key);
            lock (gate)
            {
                if (MemoryCache.Default[key] != null)
                    MemoryCache.Default.Remove(key);
            }
            //_logger.LogDebugMessage(this.GetType(), "{0} removed from cache", key);
        }
    }
}