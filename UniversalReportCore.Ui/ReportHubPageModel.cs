using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.Helpers;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui.ViewModels;

namespace UniversalReportCore.Ui
{
    public class ReportHubPageModel : PageModel
    {
        private readonly IPageMetaFactory _pageMetaFactory;

        public List<ReportLinkViewModel> Reports { get; set; }

        public ReportHubPageModel(IPageMetaFactory pageMetaFactory) 
        {
            _pageMetaFactory = pageMetaFactory;
        }

        public async Task<IActionResult> ReportHubPageGetAsync()
        {
            Reports = _pageMetaFactory.Providers
                .Select(provider =>
                {
                    var meta = provider.GetPageMeta();
                    return new ReportLinkViewModel("/Reports/Index", provider.Slug, meta.Title, meta.Subtitle, provider.Description, StringHelper.SplitPascalCase(provider.CategorySlug));
                })
                .OrderBy(item => item.Category)
                .ThenBy(item => item.Subtitle)
                .ThenBy(item => item.Title)
                .ToList();

            return Page();
        }
    }
}
