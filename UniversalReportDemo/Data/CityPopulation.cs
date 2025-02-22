using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversalReportDemo.Data
{
    // https://datahub.io/collections/demographics
    // https://datahub.io/core/population-city

    public class CityPopulation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure auto-increment if using a relational DB
        public int Id { get; set; }  // Primary Key
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

        public virtual ICollection<CityPopulationCohort> CityPopulationCohorts { get; set; } = new List<CityPopulationCohort>();
    }
}
