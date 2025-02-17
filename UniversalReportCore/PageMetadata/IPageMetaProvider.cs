
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.PageMetadata
{
    public interface IPageMetaProvider
    {
        string Slug { get; }
        string CategorySlug { get; }
        PageMetaViewModel GetPageMeta();
        ChartMetaViewModel? GetChartMeta();
    }
}
