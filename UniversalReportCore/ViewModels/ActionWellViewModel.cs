using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

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

        public IReportQueryParams? Params { get; set; }

        public string? CurrentSort { get; set; }

        public string? ResetFiltersButtonHtml { get; set; } = "Reset";
    }
}
