using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.PageMetadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageMetaPolicyAttribute : Attribute
    {
        public string Policy { get; }

        public PageMetaPolicyAttribute(string policy)
        {
            Policy = policy;
        }
    }
}
