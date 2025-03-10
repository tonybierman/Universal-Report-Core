using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UniversalReportCore;
namespace UniversalReportCore
{
    public class FilterFactory<T>
    {
        private readonly IFilterProvider<T> _provider;

        public FilterFactory(IFilterProvider<T> provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Expression<Func<T, bool>> BuildPredicate(IEnumerable<string> selectedKeys)
        {
            var finalPredicate = PredicateBuilder.New<T>(true);

            foreach (var facetGroup in _provider.GetFacetKeys())
            {
                var orPredicate = PredicateBuilder.New<T>(false);

                foreach (var key in facetGroup.Value)
                {
                    if (selectedKeys.Contains(key) && _provider.Filters.TryGetValue(key, out var filterExpression))
                    {
                        orPredicate = orPredicate.Or(filterExpression);
                    }
                }

                if (!orPredicate.IsStarted)
                    continue;

                finalPredicate = finalPredicate.And(orPredicate);
            }

            return finalPredicate;
        }
    }
}
