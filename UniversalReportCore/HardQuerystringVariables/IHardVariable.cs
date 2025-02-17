namespace UniversalReportCore.HardQuerystringVariables
{
    public interface IHardVariable
    {
        bool IsHard { get; }
        bool IsSane { get;  }
        bool IsValid { get; }

        bool CheckSanity();
    }
}