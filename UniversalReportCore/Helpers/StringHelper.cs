using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Helpers
{
    public static class StringHelper
    {
        public static string SplitPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new System.Text.StringBuilder();
            result.Append(input[0]); // Keep first char as is

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result.Append(' ');
                }
                result.Append(input[i]);
            }

            return result.ToString();
        }
    }
}
