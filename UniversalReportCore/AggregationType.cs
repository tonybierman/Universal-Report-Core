using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public enum AggregationType
    {
        None,   // Default: No aggregation
        Sum,    // Sum values
        Average, // Compute average
        Min,    // Find minimum value
        Max,    // Find maximum value
        Count,   // Count the number of records
        StandardDeviation
    }
}
