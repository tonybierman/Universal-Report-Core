using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using UniversalReportCore;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.HardQuerystringVariables.Hardened;
using UniversalReportCore.Helpers;
using UniversalReportCore.PagedQueries;
using UniversalReportCore.PageMetadata;
using UniversalReportCore.Ui;
using UniversalReportCore.ViewModels;
using Microsoft.CSharp.RuntimeBinder;
using UniversalReportCore.Ui.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace UniversalReportCore.Ui.Pages
{
    public class ReportPageModel : PageModel
    {
        // Fields
        protected readonly IMapper _mapper;
        private readonly IReportColumnFactory _reportColumnFactory;
        private readonly IReportPageHelperFactory _pageHelperFactory;
        protected readonly IPageMetaFactory _pageMetaFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        protected readonly ILogger _logger;

        // Querystring Properties
        [BindProperty(SupportsGet = true)]
        [ModelBinder(BinderType = typeof(ReportQueryParamsBinderBase))]
        public virtual IReportQueryParams Params { get; set; }
        [BindProperty(SupportsGet = true)] public string? Id { get; set; }
        
        // Properties
        [BindProperty] public PageMetaViewModel PageMeta { get; set; }
        public List<IReportColumnDefinition> ReportColumns { get; set; } = new();
        public string CurrentSort { get; set; }
        public ICohort[] Cohorts { get; set; }
        public IPaginatedList? Items { get; protected set; }
        [BindProperty] public long[]? SelectedIds { get; set; }
        public List<(string Heading, List<SelectListItem> Options)> FilterOptions { get; private set; }
        public string? SelectedFilter { get; set; }
        public bool HasFiltersAvailable { get; set; }
        public string? ActionWellPartial { get; set; }
        public ReportPageModel(
            ILogger<ReportPageModel> logger,
            IMapper mapper,
            IPageMetaFactory pageMetaFactory,
            IReportColumnFactory reportColumnFactory,
            IReportPageHelperFactory pageHelperFactory,
            IAuthorizationService authorizationService,
            IAuthorizationPolicyProvider policyProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pageMetaFactory = pageMetaFactory;
            _pageHelperFactory = pageHelperFactory;
            _reportColumnFactory = reportColumnFactory;
            _mapper = mapper;
            PageMeta = new PageMetaViewModel { Title = "Demo", Subtitle = "Lorem Ipsum" };
            _authorizationService = authorizationService;
            _policyProvider = policyProvider;
        }

        public async Task<IActionResult> ReportPageGetAsync()
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
                PageMeta = _pageMetaFactory.GetPageMeta(this.Params.Slug.Value, out var policy);

                // Check authorization if a policy is specified
                if (!string.IsNullOrEmpty(policy))
                {
                    // Validate policy existence
                    try
                    {
                        var policyObject = await _policyProvider.GetPolicyAsync(policy);
                        if (policyObject == null)
                        {
                            // Unknown policy
                            _logger.LogDebug("Invalid authorization policy configured.");
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {
                        // Handle any unexpected errors in policy retrieval
                        _logger.LogDebug("Error validating authorization policy.");
                        return StatusCode(500);
                    }

                    var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, policy);
                    if (!authorizationResult.Succeeded)
                    {
                        return Challenge();
                    }
                }
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

        public async Task<IActionResult> ReportPageGetAsync(string slug, string? displayKey = null)
        {
            if (Params.FilterKeys.Value == null)
            {
                Params.FilterKeys = new HardenedFilterKeys(new List<string> { SelectedFilter }.ToArray());
            }
            else
            {
                var filterList = Params.FilterKeys.Value.ToList();
                if (filterList.Contains(SelectedFilter))
                {
                    // Remove if it already exists
                    filterList.Remove(SelectedFilter);
                }
                else
                {
                    // Add if not present
                    if (SelectedFilter != null)
                    {
                        filterList.Add(SelectedFilter);
                    }
                }
                if (filterList != null)
                {
                    Params.FilterKeys = new HardenedFilterKeys(filterList.ToArray());
                }
            }

            // Page Helper
            var pageHelper = _pageHelperFactory.GetHelper(Params.Slug.ReportType);

            // Define report columns dynamically based on the query type
            ReportColumns = pageHelper.GetReportColumns(Params.Slug.Value);

            // Are there any filters available
            HasFiltersAvailable = pageHelper.HasFilters(ReportColumns);

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

            var preQueryArgs = new PreQueryArguments(
                slug,
                ReportColumns.ToArray(),
                Params.Pi.Value,
                CurrentSort,
                Params.Ipp.Value,
                Params.CohortIds.Value,
                Params.FilterKeys.Value
            );

            // Load data
            var parameters = pageHelper.CreateQueryParameters(preQueryArgs);
            parameters.DisplayKey = displayKey;
            if (!Params.FilterKeys.Validate(pageHelper.FilterProvider))
                return StatusCode(422); // "Invalid filter provider key"
            //parameters.FilterKeys = Params.FilterKeys.Value;
            parameters.ShouldAggregate = TempDataHelper.ShouldRecalculateAggregates(TempData, slug, Params.CohortIds.Value, Params.FilterKeys.Value);

            int totalCount = parameters.ShouldAggregate ? 0 : (TempData["TotalCount"] as int?) ?? 0;

            Items = (IPaginatedList)await pageHelper.GetPagedDataAsync(parameters, totalCount);

            if (Items == null)
            {
                return Page();
            }

            TempData["TotalCount"] = Items.TotalItems;

            // Paging Index Validation
            int totalPages = Items?.TotalPages ?? 0;
            if (!Params.Pi.Validate(totalPages))
                return StatusCode(422); // "Paging index exceeds total available pages."

            // Final check
            if (Params.IsHard)
            {
                FilterOptions = pageHelper.GetFilterSelectList(Params.FilterKeys.Value);

                if (!parameters.ShouldAggregate)
                {
                    var agg = TempDataHelper.DeserializeAggregatesFromTempData(TempData);
                    Items?.EnsureAggregates(agg);
                }

                TempDataHelper.SerializeAggregatesToTempData(TempData, Items?.Aggregates);

                return Page();
            }

            return StatusCode(400);// "Invalid value for querystring variable.");
        }

        public virtual async Task<IActionResult> OnPostBulkActionAsync(string bulkAction)
        {
            throw new NotImplementedException();
        }
    }
}
