using System.Diagnostics;

namespace UniversalReportCore.HardQuerystringVariables
{
    public class BaseHardenedVariable : IHardVariable
    {
        public bool IsSane { get; protected set; }
        public bool IsValid { get; protected set; }
        public bool IsHard { get => IsSane && IsValid; }

        public virtual bool CheckSanity()
        {
            IsSane = true;
            return IsSane;
        }
    }
}
