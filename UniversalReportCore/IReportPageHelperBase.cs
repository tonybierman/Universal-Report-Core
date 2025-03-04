﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalReportCore.Data;
using UniversalReportCore.PagedQueries;

namespace UniversalReportCore
{
    public interface IReportPageHelperBase
    {
        string DefaultSort { get; }
        Task<object> GetPagedDataAsync(PagedQueryParametersBase parameters, int totalCount = 0);
        List<IReportColumnDefinition> GetReportColumns(string slug);
        Task<ICohort[]?> GetCohortsAsync(int[] cohortIds);
        PagedQueryParametersBase CreateQueryParameters(string queryType, IReportColumnDefinition[] columns, int? pageIndex, string? sort, int? ipp, int[]? cohortIds);
        List<ChartDataPoint> GetChartData(IPaginatedList items, string key);
    }
}
