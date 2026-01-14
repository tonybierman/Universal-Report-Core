using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    /// <summary>
    /// Represents a view model for a faceted browser UI component, supporting filtering, pagination, and sorting.
    /// </summary>
    public class FacetedBrowserViewModel
    {
        public IReportColumnDefinition[]? ReportColumns { get; set; }

        public List<(string Heading, List<SelectListItem> Options)>? FilterOptions { get; set; }

        /// <summary>
        /// Gets or sets the query parameters for the report.
        /// </summary>
        /// <value>
        /// An object implementing <see cref="IReportQueryParams"/> that defines the parameters used to query the report data.
        /// </value>
        public IReportQueryParams? Params { get; set; }

        public bool ShowFilterButton { get; set; }
        public string? FilterButtonHtml { get; set; } = "Filters";

        /// <summary>
        /// Gets or sets the sort order for the displayed data.
        /// </summary>
        /// <value>
        /// A string indicating the sort order (e.g., "asc" or "desc"). Null if no sort order is specified.
        /// </value>
        public string? CurrentSort { get; set; }
    }
}