using UniversalReportCore.PagedQueries;
using UniversalReportCoreTests.Data;

namespace UniversalReportCore.Tests.Reports.Acme
{
    public class AcmeQueryFactory : QueryFactory<Widget>
    {
        public AcmeQueryFactory(IEnumerable<IPagedQueryProvider<Widget>> providers) : base(providers)
        {
        }
    }
}
