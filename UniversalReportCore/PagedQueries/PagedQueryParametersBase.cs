using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    public class PagedQueryParametersBase
    {
        /// <summary>
        /// Gets or sets an optional SKU filter, if applicable.
        /// </summary>
        public string? Sku { get; set; } // TODO: Rename this to be a primary column indicator

        /// <summary>
        /// Gets or sets the index of the page to be retrieved.
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the sorting parameter for ordering the query results.
        /// Example values: "ColumnName", "ColumnNameDesc" (for descending order).
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Gets or sets an optional date range filter for queries.
        /// </summary>
        public DateRangeFilter? DateFilter { get; set; }

        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        public int? ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets an array of cohort identifiers for filtering data.
        /// </summary>
        public int[]? CohortIds { get; set; }
    }
}
