namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents a hardened query string variable with validation and sanity checks.
    /// </summary>
    public interface IHardVariable
    {
        /// <summary>
        /// Gets a value indicating whether the variable is both sane and valid.
        /// </summary>
        bool IsHard { get; }

        /// <summary>
        /// Gets a value indicating whether the variable passes sanity checks.
        /// Sanity checks ensure the value is within expected boundaries.
        /// </summary>
        bool IsSane { get; }

        /// <summary>
        /// Gets a value indicating whether the variable is valid.
        /// Validity checks ensure the value conforms to domain rules.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Performs a sanity check on the variable and updates its state.
        /// </summary>
        /// <returns><c>true</c> if the value is sane; otherwise, <c>false</c>.</returns>
        bool CheckSanity();
    }
}
