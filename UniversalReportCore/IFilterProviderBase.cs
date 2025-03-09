using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public interface IFilterProviderBase
    {
        Dictionary<string, List<string>> GetFacetKeys();
    }
}
