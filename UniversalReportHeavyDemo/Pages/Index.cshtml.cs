using Microsoft.AspNetCore.Mvc;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Pages
{
    public class IndexModel : ReportHubPageModel
    {
        public IndexModel(IPageMetaFactory pageMetaFactory) : base(pageMetaFactory) { }

        public async Task<IActionResult> OnGetAsync()
        {
            return await ReportHubPageGetAsync();
        }
    }
}
