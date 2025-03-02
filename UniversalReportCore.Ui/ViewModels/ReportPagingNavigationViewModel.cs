using System;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    /// <summary>
    /// Represents the view model for paging navigation in a report view.
    /// Provides the current sort order, the active query parameters, and the paginated list of items.
    /// </summary>
    public class ReportPagingNavigationViewModel
    {
        /// <summary>
        /// Gets or sets the current sort order of the report (e.g., "NameAsc" or "DateDesc").
        /// </summary>
        public string? CurrentSort { get; set; }

        /// <summary>
        /// Gets or sets the current report query parameters.
        /// Contains paging, sorting, and cohort information.
        /// </summary>
        public IReportQueryParams Params { get; set; } = default!;

        /// <summary>
        /// Gets or sets the paginated list of items being displayed.
        /// </summary>
        public IPaginatedList Items { get; set; } = default!;
    }
}
