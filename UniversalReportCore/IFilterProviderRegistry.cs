using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public interface IFilterProviderRegistry<T>
    {
        IFilterProvider<T> GetProvider(string key);
        IEnumerable<IFilterProvider<T>> GetAllProviders();
    }
}
