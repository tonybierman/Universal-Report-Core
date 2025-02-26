namespace UniversalReportCore
{
    public interface IReportPageHelperFactory
    {
        IReportPageHelperBase GetHelper(string reportType);
    }
}