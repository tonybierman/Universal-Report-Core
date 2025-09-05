using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    public class SearchViewModel
    {
        public List<(string Heading, List<SelectListItem> Options)>? FilterOptions { get; set; }

        public IReportColumnDefinition[]? ReportColumns { get; set; }

        public IReportQueryParams Params { get; set; }

        public string? CurrentSort { get; set; }
    }
}