using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalReportCore
{
    /// <summary>
    /// Maintains a registry of filter providers, allowing retrieval based on a unique key.
    /// </summary>
    /// <typeparam name="T">The type of entity that the filter providers operate on.</typeparam>
    public class FilterProviderRegistry<T> : IFilterProviderRegistry<T>
    {
        private readonly IDictionary<string, IFilterProvider<T>> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterProviderRegistry{T}"/> class.
        /// </summary>
        /// <param name="providers">A collection of filter providers to be registered.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="providers"/> is null.</exception>
        public FilterProviderRegistry(IEnumerable<IFilterProvider<T>> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers), "Providers collection cannot be null.");
            }

            _providers = providers.ToDictionary(p => p.Key, p => p);
        }

        /// <summary>
        /// Retrieves the filter provider registered under the specified key.
        /// </summary>
        /// <param name="key">The unique key identifying the filter provider.</param>
        /// <returns>The <see cref="IFilterProvider{T}"/> associated with the given key.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if no filter provider is registered with the specified key.
        /// </exception>
        public IFilterProvider<T> GetProvider(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null.");
            }

            if (_providers.TryGetValue(key, out var provider))
            {
                return provider;
            }

            throw new KeyNotFoundException($"No filter provider registered with key '{key}'.");
        }
    }
}
