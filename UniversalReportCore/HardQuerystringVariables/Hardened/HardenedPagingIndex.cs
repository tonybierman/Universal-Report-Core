using UniversalReportCore.HardQuerystringVariables;

namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedPagingIndex : HardenedVariable<int?>
    {
        private readonly int _min = 1;
        private readonly int _max = 10000;

        public HardenedPagingIndex(int? index) : base(index) { }

        public override bool CheckSanity()
        {
            // Null values are considered valid.
            IsSane = Value == null || Value >= _min && Value <= _max;
            return IsSane;
        }

        public bool Validate(int totalPages)
        {
            // Null values are valid.
            IsValid = Value == null || Value <= totalPages;
            return IsValid;
        }
    }
}
