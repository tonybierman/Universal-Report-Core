using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents the base class for hardened query string variables.
    /// Ensures variables are checked for validity and sanity before being used.
    /// </summary>
    public class BaseHardenedVariable : IHardVariable
    {
        public BaseHardenedVariable() { }

        /// <summary>
        /// Gets a value indicating whether the variable passes sanity checks.
        /// </summary>
        public bool IsSane { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the variable is considered valid.
        /// </summary>
        public bool IsValid { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the variable is both sane and valid.
        /// </summary>
        public bool IsHard => IsSane && IsValid;

        /// <summary>
        /// Performs a sanity check on the variable.
        /// </summary>
        /// <returns>True if the variable is considered sane; otherwise, false.</returns>
        public virtual bool CheckSanity()
        {
            IsSane = true;
            return IsSane;
        }

        protected bool IsSafeQueryString(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            if (input.Length > 2048) return false;

            string[] sqlPatterns = {
        "--",
        "\\b(union|select|insert|delete|update|drop|alter|create|truncate|exec|execute)\\b",
        "[;']",
        "\\|\\|",
        "/\\*",
        "\\*/"
    };

            string[] xssPatterns = {
        "<script",
        "javascript:",
        "onerror=",
        "onload=",
        "<iframe",
        "<img",
        "eval\\(",
        "expression\\(",
        "vbscript:",
        "<svg",
        "<object",
        "<embed"
    };

            string pattern = $"({string.Join("|", sqlPatterns.Concat(xssPatterns))})";

            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                return false;

            if (input.Contains("%") || input.Contains("&"))
                return false;

            if (!Regex.IsMatch(input, @"^[a-zA-Z0-9\s_.,= -]*$"))
                return false;

            return true;
        }
    }
}
