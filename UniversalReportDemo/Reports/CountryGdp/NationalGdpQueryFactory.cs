using UniversalReportCore.PagedQueries;
using UniversalReportDemo.Data;

namespace UniversalReportDemo.Reports.CountryGdp
{
    public class NationalGdpQueryFactory : QueryFactory<NationalGdp>
    {
        public NationalGdpQueryFactory(IEnumerable<IPagedQueryProvider<NationalGdp>> providers) : base(providers)
        {
        }
    }
}
