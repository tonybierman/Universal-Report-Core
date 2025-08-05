using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UniversalReportCore.ViewModels;

namespace UniversalReportCoreTests.ViewModels
{
    public class WidgetViewModel : BaseEntityViewModel, IEntityViewModel<int>
    {
        public string? CountryOrArea { get; set; }
        public int? Year { get; set; }
        public string? Area { get; set; }
        public string? Sex { get; set; }
        public string? City { get; set; }
        public string? CityType { get; set; }
        public string? RecordType { get; set; }
        public string? Reliability { get; set; }
        public int? SourceYear { get; set; }
        public decimal? Value { get; set; }
        public string? ValueFootnotes { get; set; }
    }
}
