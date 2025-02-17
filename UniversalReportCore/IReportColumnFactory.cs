namespace UniversalReportCore
{
    public interface IReportColumnFactory
    {
        List<IReportColumnDefinition> GetReportColumns(string slug);
    }

}
