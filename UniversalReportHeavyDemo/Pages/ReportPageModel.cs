using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.Helpers;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;
using UniversalReportCore.ViewModels;
using UniversalReportHeavyDemo.Reports;

namespace UniversalReportHeavyDemo.Pages
{
    public class ReportPageModel : PageModel
    {
        // Fields
        private readonly IMapper _mapper;
        private readonly IReportColumnFactory _reportColumnFactory;
        private readonly PageHelperFactory _pageHelperFactory;
        protected readonly IPageMetaFactory _pageMetaFactory;
        protected readonly ILogger _logger;

        // Querystring Properties
        [BindProperty(SupportsGet = true)]
        [ModelBinder(BinderType = typeof(ReportQueryParamsBinder))]
        public ReportQueryParams Params { get; set; }
        [BindProperty(SupportsGet = true)] public string? Id { get; set; }
        
        // Properties
        [BindProperty] public PageMetaViewModel PageMeta { get; set; }
        public List<IReportColumnDefinition> ReportColumns { get; set; } = new();
        public string CurrentSort { get; set; }
        public ICohort[] Cohorts { get; set; }
        public IPaginatedList? Items { get; protected set; }
        [BindProperty] public long[]? SelectedIds { get; set; }

        public ReportPageModel(
            ILogger logger,
            IMapper mapper,
            IPageMetaFactory pageMetaFactory,
            IReportColumnFactory reportColumnFactory,
            PageHelperFactory pageHelperFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageMetaFactory = pageMetaFactory;
            _pageHelperFactory = pageHelperFactory;
            _reportColumnFactory = reportColumnFactory;
            _mapper = mapper;
            PageMeta = new PageMetaViewModel { Title = "Demo", Subtitle = "Lorem Ipsum" };
        }
        
        public async Task<IActionResult> ReportPageGetAsync(string slug)
        {
            // Page Helper
            var pageHelper = _pageHelperFactory.GetHelper(Params.Slug.ReportType);  // `dynamic` to call generic methods without reflection headaches.

            // Define report columns dynamically based on the query type
            ReportColumns = pageHelper.GetReportColumns(Params.Slug.Value);

            // Set the sort order
            Params.SortOrder = new HardenedColumnSort(Params.SortOrder.Value ?? pageHelper.DefaultSort);
            if (!(Params.SortOrder.CheckSanity() && Params.SortOrder.Validate(ReportColumns)))
            {
                return StatusCode(422);// "The specified report column does not exist."
            }
            SortHelper.ConfigureSort(ReportColumns, Params.SortOrder.Value);
            CurrentSort = Params.SortOrder.Value;

            if (Params.CohortIds.Value != null && Params.CohortIds.Value.Any())
            {
                Cohorts = await pageHelper.GetCohortsAsync(Params.CohortIds.Value);
                if (!Params.CohortIds.Validate(Cohorts))
                    return StatusCode(422); // "Unknown cohort."
            }
            else
            {
                if (!Params.CohortIds.Validate())
                    return StatusCode(400);// "Chort validation error."
            }

            // Load data
            var parameters = pageHelper.CreateQueryParameters(slug, ReportColumns.ToArray(), Params.Pi.Value, CurrentSort, Params.Ipp.Value, Params.CohortIds.Value);

            Items = (IPaginatedList)await pageHelper.GetPagedDataAsync(parameters);

            if (Items == null)
            {
                return Page();
            }

            // Paging Index Validation
            int totalPages = Items?.TotalPages ?? 0;
            if (!Params.Pi.Validate(totalPages))
                return StatusCode(422); // "Paging index exceeds total available pages."

            // Final check
            if (Params.IsHard)
            {
                return Page();
            }

            return StatusCode(400);// "Invalid value for querystring variable.");
        }

        public async Task<IActionResult> OnPostBulkActionAsync(string bulkAction)
        {
            throw new NotImplementedException();
        }
    }
}
