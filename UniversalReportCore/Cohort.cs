using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversalReportCore
{
    public partial class Cohort : ICohort
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure auto-increment if using a relational DB
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
