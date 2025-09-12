namespace UniversalReportCore
{
    using System.Collections;

    public interface IPaginatedList : IEnumerable
    {
        Type EntityViewModelType { get; }
        Dictionary<string, dynamic>? Aggregates { get; }
        string DisplayMessage { get; }
        int EndItem { get; }
        bool HasMultiplePages { get; }
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }
        Dictionary<string, dynamic>? Meta { get; }
        int PageIndex { get; }
        int PageSize { get; }
        int StartItem { get; }
        int TotalItems { get; }
        int TotalPages { get; }
        bool Any();
        int Count { get; }
        void EnsureAggregates(Dictionary<string, dynamic>? newAggregates);
    }
}
