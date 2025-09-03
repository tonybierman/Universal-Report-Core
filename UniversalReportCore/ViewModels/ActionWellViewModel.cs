using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.ViewModels
{
    public class ActionWellViewModel
    {
        public ActionWellViewModel(IList<SubPartialViewModel> subPartials)
        {
            SubPartials = new List<SubPartialViewModel>();
            SubPartials.AddRange(subPartials);
        }

        public List<SubPartialViewModel> SubPartials { get; set; }
    }
}
