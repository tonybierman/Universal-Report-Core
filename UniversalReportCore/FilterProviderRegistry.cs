using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public class FilterProviderRegistry<T> : IFilterProviderRegistry<T>
    {
        private readonly IDictionary<string, IFilterProvider<T>> _providers;

        public FilterProviderRegistry(IEnumerable<IFilterProvider<T>> providers)
        {
            _providers = providers.ToDictionary(p => p.Key, p => p);
        }

        public IFilterProvider<T> GetProvider(string key)
        {
            if (_providers.TryGetValue(key, out var provider))
            {
                return provider;
            }

            throw new KeyNotFoundException($"No filter provider registered with key '{key}'.");
        }
    }

}
