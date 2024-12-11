using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Singletone
{
    public interface IProvaCache
    {
        T AddEdit<T>(string key, Func<T> delegat);
        public void Remove(string key);
    }
    public class ProvaCache : IProvaCache
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        public T AddEdit<T>(string key, Func<T> delegat)
        {
            lock (_lock)
            {
                if (_dictionary.ContainsKey(key))
                {
                    return (T)_dictionary[key];
                }
                else
                {
                    _dictionary.Add(key, delegat());
                    return (T)_dictionary[key];
                }
            }
        }

        public void Remove(string key)
        {
            lock (_lock)
            {
                if (_dictionary.ContainsKey(key))
                {
                    _dictionary.Remove(key);
                }
            }
        }
    }
}