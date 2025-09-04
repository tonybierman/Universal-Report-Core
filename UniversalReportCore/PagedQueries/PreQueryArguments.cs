using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.PagedQueries
{
    public class PreQueryArguments
    {
        public string QueryType { get; set; }
        public IReportColumnDefinition[] Columns { get; set; }
        public int? PageIndex { get; set; }
        public string? Sort { get; set; }
        public int? Ipp { get; set; }
        public int[]? CohortIds { get; set; }
        public string[]? FilterKeys { get; set; }
        public TextFilter[]? SearchFilters { get; set; }

        public PreQueryArguments(string queryType, IReportColumnDefinition[] columns, int? pageIndex = null, string? sort = null, int? ipp = null, int[]? cohortIds = null, string[]? filterKeys = null)
        {
            QueryType = queryType;
            Columns = columns;
            PageIndex = pageIndex;
            Sort = sort;
            Ipp = ipp;
            CohortIds = cohortIds;
            FilterKeys = filterKeys;
        }
    }
}
