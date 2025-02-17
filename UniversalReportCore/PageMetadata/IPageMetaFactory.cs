
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    public interface IPageMetaFactory
    {
        IEnumerable<IPageMetaProvider> Providers { get; }
        PageMetaViewModel GetPageMeta(string slug);
        ChartMetaViewModel? GetChartMeta(string slug);
    }
}
