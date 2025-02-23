using UniversalReportCore.PagedQueries;
using UniversalReportHeavyDemo.Data;

namespace UniversalReportHeavyDemo.Reports.CountryGdp
{
    public class NationalGdpQueryFactory : QueryFactory<NationalGdp>
    {
        public NationalGdpQueryFactory(IEnumerable<IPagedQueryProvider<NationalGdp>> providers) : base(providers)
        {
        }
    }
}
