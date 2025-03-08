namespace UniversalReportCore.Ui
{
    public interface IReportPageHelperFactory
    {
        IReportPageHelperBase GetHelper(string reportType);
    }
}