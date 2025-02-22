using System.Diagnostics;

namespace UniversalReportCore.HardQuerystringVariables
{
    /// <summary>
    /// Represents a strongly typed hardened query string variable.
    /// Provides a base for enforcing sanity and validation checks on query parameters.
    /// </summary>
    /// <typeparam name="T">The type of the variable's value.</typeparam>
    [DebuggerDisplay("{Value}, IsHard = {IsHard}")]
    public class HardenedVariable<T> : BaseHardenedVariable
    {
        public HardenedVariable() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HardenedVariable{T}"/> class.
        /// </summary>
        /// <param name="value">The initial value of the variable.</param>
        public HardenedVariable(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value of the hardened variable.
        /// </summary>
        public T Value { get; set; }
    }
}
