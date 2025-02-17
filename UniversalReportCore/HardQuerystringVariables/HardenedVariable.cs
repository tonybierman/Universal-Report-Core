using System.Diagnostics;

namespace UniversalReportCore.HardQuerystringVariables
{
    [DebuggerDisplay("{Value}, IsHard = {IsHard}")]
    public class HardenedVariable<T> : BaseHardenedVariable
    {
        public HardenedVariable(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
