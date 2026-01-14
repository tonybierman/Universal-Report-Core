using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.ViewModels
{
    public class SubPartialViewModel
    {
        public required string PartialName { get; set; } // Name of the sub-partial (e.g., "_SubPartial1", "_SubPartial2")
        public required object Data { get; set; } // View model for the sub-partial
    }
}
