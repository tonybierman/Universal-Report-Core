using UniversalReportCore;

public class FilterProviderRegistry<T> : IFilterProviderRegistry<T>
{
    private readonly IDictionary<Type, IFilterProvider<T>> _providers;

    public FilterProviderRegistry(IEnumerable<IFilterProvider<T>> providers)
    {
        _providers = providers.ToDictionary(p => p.GetType(), p => p);
    }

    public IFilterProvider<T> GetProvider()
    {
        return _providers.Values.FirstOrDefault()
            ?? throw new InvalidOperationException($"No filter provider registered for entity type '{typeof(T).Name}'.");
    }

    public IEnumerable<IFilterProvider<T>> GetAllProviders()
    {
        return _providers.Values;
    }
}
