using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    public class ReportPagingNavigationViewModel
    {
        public string? CurrentSort { get; set; }
        public IReportQueryParams Params { get; set; } = default!;
        public IPaginatedList Items { get; set; } = default!;
    }
}

