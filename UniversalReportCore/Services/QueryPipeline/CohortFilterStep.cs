using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore.Services.QueryPipeline
{
    public class CohortFilterStep<TEntity> : IQueryPipelineStep<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Execute(IQueryable<TEntity> query, PagedQueryParameters<TEntity> parameters)
        {
            if (parameters.CohortIds != null && parameters.CohortIds.Any() && parameters.CohortLogic != null)
            {
                query = parameters.CohortLogic(query, parameters.CohortIds);
            }
            return query;
        }
    }
}
