using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class ReportFilterStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            return parameters.ReportFilter != null ? parameters.ReportFilter(query) : query;
        }
    }
}
