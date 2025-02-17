namespace UniversalReportCore
{
    public interface IReportColumnProvider
    {
        string Slug { get; }
        List<IReportColumnDefinition> GetColumns();
    }
}
