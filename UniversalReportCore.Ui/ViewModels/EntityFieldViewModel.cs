using Microsoft.AspNetCore.Mvc;
using UniversalReportCore.HardQuerystringVariables;
using UniversalReportCore.ViewModels;

namespace UniversalReportCore.Ui.ViewModels
{
    /// <summary>
    /// Provides a base view model for rendering entity fields within a report.
    /// </summary>
    public class EntityFieldViewModel
    {
        /// <summary>
        /// Gets the parent entity containing the field data.
        /// </summary>
        public IEntityViewModel<int> Parent { get; }

        /// <summary>
        /// Gets the column definition for the field.
        /// </summary>
        public IReportColumnDefinition Column { get; }

        // Querystring Properties
        public virtual IReportQueryParams? Params { get; set; }

        /// <summary>
        /// Gets or sets the token used for sort order.
        /// </summary>
        public string? CurrentSort { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent entity containing the field data.</param>
        /// <param name="column">The report column definition.</param>
        /// <param name="queryParams">The query parameters used for routing or identification.</param>
        /// <param name="currentSort">The token used for sort order.</param>
        public EntityFieldViewModel(IEntityViewModel<int> parent, IReportColumnDefinition column, IReportQueryParams? queryParams, string? currentSort)
        {
            Parent = parent;
            Column = column;
            Params = queryParams;
            CurrentSort = currentSort;
        }
    }
}
