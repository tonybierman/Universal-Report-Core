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
        /// <summary>
        /// Gets or sets the query parameters for the report.
        /// </summary>
        /// <value>
        /// An object implementing <see cref="IReportQueryParams"/> that defines the parameters used to query the report data.
        /// </value>
        public IReportQueryParams Params { get; set; }

        /// <summary>
        /// Gets or sets the list of filter options, each consisting of a heading and a collection of selectable items.
        /// </summary>
        /// <value>
        /// A list of tuples where each tuple contains a string heading and a list of <see cref="SelectListItem"/> options.
        /// </value>
        public List<(string Heading, List<SelectListItem> Options)> FilterOptions { get; set; }

        /// <summary>
        /// Gets or sets the current page index for pagination.
        /// </summary>
        /// <value>
        /// An nullable integer representing the current page index (0-based). Null if pagination is not applicable.
        /// </value>
        public int? PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the sort order for the displayed data.
        /// </summary>
        /// <value>
        /// A string indicating the sort order (e.g., "asc" or "desc"). Null if no sort order is specified.
        /// </value>
        public string? SortOrder { get; set; }
    }
}