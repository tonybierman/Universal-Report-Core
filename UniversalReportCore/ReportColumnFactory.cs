using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversalReportCore
{
    /// <summary>
    /// Factory class responsible for retrieving report column definitions
    /// from registered column providers based on a specified report slug.
    /// </summary>
    public class ReportColumnFactory : IReportColumnFactory
    {
        private readonly IEnumerable<IReportColumnProvider> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportColumnFactory"/> class.
        /// </summary>
        /// <param name="providers">A collection of <see cref="IReportColumnProvider"/> implementations.</param>
        public ReportColumnFactory(IEnumerable<IReportColumnProvider> providers)
        {
            _providers = providers;
        }

        /// <summary>
        /// Retrieves the list of report columns for the specified report type (slug).
        /// </summary>
        /// <param name="slug">The identifier for the report type.</param>
        /// <returns>A list of <see cref="IReportColumnDefinition"/> objects defining the report's columns.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no provider exists for the given slug.
        /// </exception>
        public List<IReportColumnDefinition> GetReportColumns(string slug)
        {
            var provider = _providers.FirstOrDefault(p => p.Slug == slug);
            if (provider == null)
            {
                throw new InvalidOperationException($"Unsupported report type: {slug}");
            }

            return provider.GetColumns();
        }
    }
}
