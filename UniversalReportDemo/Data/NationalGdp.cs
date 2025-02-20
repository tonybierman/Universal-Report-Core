using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversalReportDemo.Data
{
    // https://datahub.io/core/gdp
    // GDP in current USD
    public class NationalGdp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure auto-increment if using a relational DB
        public int Id { get; set; }  // Primary Key
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public int? Year { get; set; }
        public double? Value { get; set; }
    }
}
