using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    public class ItemsPerPageSelectorViewModel
    {
        /// <summary>
        /// Gets or sets the query parameters for the report.
        /// </summary>
        public ReportQueryParams Params { get; set; }

        /// <summary>
        /// Gets or sets the paginated list of items.
        /// </summary>
        public IPaginatedList Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsPerPageSelectorViewModel"/> class.
        /// </summary>
        /// <param name="params">The report query parameters.</param>
        /// <param name="items">The paginated list of items.</param>
        public ItemsPerPageSelectorViewModel(ReportQueryParams @params, IPaginatedList items)
        {
            Params = @params;
            Items = items;
        }
    }
}
