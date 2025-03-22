using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.Ui.ViewModels
{
    public class ReportTableViewModel
    {
        public IPaginatedList? Items { get; }
        public IReportQueryParams Params { get; }
        public List<IReportColumnDefinition> ReportColumns { get; }
        public string CurrentSort { get; }
        public bool HasFiltersAvailable { get; }
        public List<(string Heading, List<SelectListItem> Options)> FilterOptions { get; }
        public ICohort[] Cohorts { get; }

        public ReportTableViewModel()
        {
        }

        public ReportTableViewModel(IPaginatedList? paginatedList, IReportQueryParams reportQueryParams, List<IReportColumnDefinition> reportColumnDefinitions, string currentSort, bool hasFiltersAvailable, List<(string Heading, List<SelectListItem> Options)> filterOptions, ICohort[] cohorts)
        {
            Items = paginatedList;
            Params = reportQueryParams;
            ReportColumns = reportColumnDefinitions;
            CurrentSort = currentSort;
            HasFiltersAvailable = hasFiltersAvailable;
            FilterOptions = filterOptions;
            Cohorts = cohorts;
        }
    }
}
