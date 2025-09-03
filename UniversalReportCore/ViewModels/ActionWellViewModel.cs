using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.ViewModels
{
    public class ActionWellViewModel
    {
        public IEnumerable<SubPartialViewModel> SubPartials { get; set; } = Enumerable.Empty<SubPartialViewModel>();
    }
}
