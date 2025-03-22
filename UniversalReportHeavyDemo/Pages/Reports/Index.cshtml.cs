using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UniversalReportCore.Helpers;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.ViewModels;
using UniversalReportCore;
using UniversalReportHeavyDemo.Reports;
using UniversalReportCore.Ui.Pages;
using UniversalReportCore.HardQuerystringVariables;
using Microsoft.CSharp.RuntimeBinder;
using UniversalReportCore.Ui;
using UniversalReportCore.Ui.ViewModels;

namespace UniversalReportHeavyDemo.Pages.Reports
{
    public class IndexModel : ReportPageModel
    {
        public IndexModel(ILogger<IndexModel> logger,
            IMapper mapper,
            IReportColumnFactory reportColumnFactory,
            IPageMetaFactory pageMetaFactory,
            IReportPageHelperFactory pageHelperFactory) :
            base(logger, mapper, pageMetaFactory,
                reportColumnFactory, pageHelperFactory)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await ReportPageGetAsync();
        }
    }
}
