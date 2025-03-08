using UniversalReportCore;

public interface IFilterProviderRegistry<T>
{
    IFilterProvider<T> GetProvider(); // Remove <TFilter> from method signature
    IEnumerable<IFilterProvider<T>> GetAllProviders();
}
