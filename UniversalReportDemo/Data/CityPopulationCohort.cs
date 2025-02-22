using System.Collections.Generic;
using UniversalReportCore;

namespace UniversalReportDemo.Data
{
    public partial class CityPopulationCohort : Cohort
    {
        public virtual ICollection<CityPopulation> CityPopulations { get; set; } = new List<CityPopulation>();
    }
}
