using System;
using System.Runtime.Caching;

namespace Performance.Extensions
{
    public class LazyCache
    {

        private static MemoryCache _cache = new MemoryCache("ExampleCache");

        public static object GetItem(string key)
        {
            return AddOrGetExisting(key, () => InitItem(key));
        }

        private static T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = _cache.AddOrGetExisting(key, newValue, new CacheItemPolicy()) as Lazy<T>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache. Thanks to Denis Borovnev for pointing this out!
                _cache.Remove(key);
                throw;
            }
        }

        private static object InitItem(string key)
        {
            // Do something expensive to initialize item
            return new { Value = key.ToUpper() };
        }


    }
}