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

        /// <summary>
        /// Gets or sets the slug used for routing or identification.
        /// </summary>
        public string? Slug { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFieldViewModel"/> class.
        /// </summary>
        /// <param name="parent">The parent entity containing the field data.</param>
        /// <param name="column">The report column definition.</param>
        public EntityFieldViewModel(IEntityViewModel<int> parent, IReportColumnDefinition column)
        {
            Parent = parent;
            Column = column;
        }
    }
}
