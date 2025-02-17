using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedItemsPerPage : HardenedVariable<int?>
    {
        private readonly int _min = 0;
        private readonly int _max = 10000;

        public HardenedItemsPerPage(int? ipp) : base(ipp) { }

        public override bool CheckSanity()
        {
            if (Value == null)
            {
                IsSane = true; // Null values are considered valid.
            }
            else
            {
                IsSane = Value >= _min && Value <= _max;
            }

            // No validity check needed
            IsValid = IsSane;

            return IsSane;
        }
    }
}
