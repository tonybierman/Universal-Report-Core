using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Base interface for filter providers, defining the contract for retrieving facet keys.
    /// </summary>
    public interface IFilterProviderBase
    {
        /// <summary>
        /// Retrieves a dictionary of facet categories and their corresponding filter keys.
        /// </summary>
        /// <returns>
        /// A dictionary where:
        /// - Keys represent facet names (e.g., "Category", "Status").
        /// - Values are lists of available filter keys within each facet.
        /// </returns>
        Dictionary<string, List<string>> GetFacetKeys();
    }
}
