using System.Collections.Generic;
using UniversalReportCore;

namespace UniversalReportHeavyDemo.Data
{
    public partial class CityPopulationCohort : Cohort, ICohort
    {
        public virtual ICollection<CityPopulation> CityPopulations { get; set; } = new List<CityPopulation>();
    }
}
