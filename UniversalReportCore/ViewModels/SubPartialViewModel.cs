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
        
        // TODO: Make this required
        public object Data { get; set; } = null!; // View model for the sub-partial
    }
}
