using System;
using System.Runtime.Caching;

namespace MajorProjects.Data
{
    using Microsoft.Owin.Logging;

    public interface ICacheService
    {
        T Get<T>(string key, Func<T> ifNotSetAddThis);
        T Get<T>(string key, Func<T> ifNotSetAddThis, int cacheInMinutes);
        void RemoveCacheByKey(string key);
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
                return (T)result;
            }
            else
            {
                lock (gate)
                {
                    cache = MemoryCache.Default;
                    result = cache[key];
                    if (result == null)
                    {
                        result = ifNotSetAddThis();
                        var policyLastUpdate = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheInMinutes) };
                        CacheByKey<T>(key, cache, result, policyLastUpdate);
                    }
                    else
                    {
                        //log error here
                    }
                    return (T)result;
                }
            }
        }

        private static void CacheByKey<T>(string key, MemoryCache cache, object result, CacheItemPolicy policyLastUpdate)
        {
            cache.Add(new CacheItem(key, result), policyLastUpdate);
        }

        public void RemoveCacheByKey(string key)
        {
            lock (gate)
            {
                if (MemoryCache.Default[key] != null)
                    MemoryCache.Default.Remove(key);
            }
        }
    }
}