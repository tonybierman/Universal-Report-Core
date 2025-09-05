
namespace UniversalReportCore.HardQuerystringVariables.Hardened
{
    public class HardenedSearchQueries
    {
        private Dictionary<string, string> searchDict;

        public HardenedSearchQueries(Dictionary<string, string> searchDict)
        {
            this.searchDict = searchDict;
        }
    }
}