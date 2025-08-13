using System.Linq.Expressions;

namespace UniversalReportCore
{
    public interface IFilterFactory<T>
    {
        Expression<Func<T, bool>> BuildPredicate(IEnumerable<string> selectedKeys);
    }
}