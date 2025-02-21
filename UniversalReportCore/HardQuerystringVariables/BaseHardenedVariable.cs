using System.Diagnostics;

namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents the base class for hardened query string variables.
    /// Ensures variables are checked for validity and sanity before being used.
    /// </summary>
    public class BaseHardenedVariable : IHardVariable
    {
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
    }
}
