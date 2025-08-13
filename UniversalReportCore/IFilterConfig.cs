namespace UniversalReportCore
{
    public interface IFilterConfig<T>
    {
        FilterFactory<T> FilterFactory { get; init; }
        string[] FilterKeys { get; init; }
        IFilterProvider<T> FilterProvider { get; init; }
    }
}