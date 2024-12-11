using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Singletone
{
    public interface ICacheService
    {
        T AddOrEdit<T>(string key, Func<T> delegat);
        public void RemoveCache(string key);
    }
    public class CacheService : ICacheService
    {
        private Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly object _lock = new object();

        public T AddOrEdit<T>(string key, Func<T> delegat)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key))
                {
                    return (T)_cache[key];
                }
                var value = delegat();
                _cache[key] = value;
                return value;
            }
        }

        public void RemoveCache(string key)
        {
            lock (_lock)
            {
                _cache.Remove(key);
            }
        }

    }
}
