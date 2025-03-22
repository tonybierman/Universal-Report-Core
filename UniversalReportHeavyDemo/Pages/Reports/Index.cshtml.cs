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
            // Check and validate query parameters
            if (!Params.CheckSanity())
            {
                _logger.LogDebug("One or more query parameters are invalid.");
                return StatusCode(422);
            }

            // A report slug is required
            if (string.IsNullOrWhiteSpace(this.Params.Slug.Value))
            {
                _logger.LogDebug("A report slug is required.");
                return NotFound();
            }

            // Slug is invalid or does not map to a known report type.
            if (!Params.Slug.Validate(_pageMetaFactory) || Params.Slug.ReportType == null)
            {
                _logger.LogDebug("Slug is invalid or does not map to a known report type.");
                return NotFound();
            }

            try
            {
                PageMeta = _pageMetaFactory.GetPageMeta(this.Params.Slug.Value);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            // Sanity check for paging index, items per page, or cohort IDs.
            if (!Params.Pi.IsSane || !Params.Ipp.IsSane || !Params.CohortIds.IsSane)
            {
                _logger.LogDebug("Sanity check for paging index, items per page, or cohort IDs.");
                return StatusCode(422);
            }

            return await ReportPageGetAsync(this.Params.Slug.Value);
        }
    }
}
