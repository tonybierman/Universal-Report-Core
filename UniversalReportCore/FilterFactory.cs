using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit; // for PredicateBuilder
using System.Linq.Expressions;

namespace UniversalReportCore
{
    public class FilterFactory<T>
    {
        public Expression<Func<T, bool>> BuildPredicate(IFilterProvider<T> provider)
        {
            var predicate = PredicateBuilder.New<T>(true); // Start with TRUE

            foreach (var andFilter in provider.GetAndFilters())
            {
                predicate = predicate.And(andFilter);
            }

            var orFilters = provider.GetOrFilters();
            if (orFilters.Any())
            {
                var orPredicate = PredicateBuilder.New<T>(false); // Start with FALSE
                foreach (var orFilter in orFilters)
                {
                    orPredicate = orPredicate.Or(orFilter);
                }

                predicate = predicate.And(orPredicate);
            }

            return predicate;
        }
    }
}
