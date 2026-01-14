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

        public List<ReportLinkViewModel>? Reports { get; set; }

        public ReportHubPageModel(IPageMetaFactory pageMetaFactory)
        {
            _pageMetaFactory = pageMetaFactory;
        }

        public async Task<IActionResult> ReportHubPageGetAsync()
        {
            Reports = _pageMetaFactory.Providers
                .Where(a => a.IsPublished)
                .Select(provider =>
                {
                    var meta = provider.GetPageMeta();
                    return new ReportLinkViewModel(provider.RouteLiteral, provider.Slug, meta.Title ?? string.Empty, meta.Subtitle ?? string.Empty, provider.Description ?? string.Empty, StringHelper.SplitPascalCase(provider.TaxonomySlug));
                })
                .OrderBy(item => item.Taxonomy)
                .ThenBy(item => item.Subtitle)
                .ThenBy(item => item.Title)
                .ToList();

            return Page();
        }
    }
}
