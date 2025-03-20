using System;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Defines the contract for commands that can deserialize data, check execution feasibility, execute asynchronously, and reset their state.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Deserializes a JSON string into the command’s internal data representation.
        /// </summary>
        /// <param name="data">The JSON string to deserialize.</param>
        /// <returns>The deserialized data as an <see cref="object"/>.</returns>
        /// <exception cref="System.Text.Json.JsonException">Thrown if <paramref name="data"/> is invalid or cannot be deserialized.</exception>
        object Deserialize(string data);

        /// <summary>
        /// Determines whether the command can be executed with its current state.
        /// </summary>
        /// <returns><c>true</c> if the command can execute; otherwise, <c>false</c>.</returns>
        bool CanExecute();

        /// <summary>
        /// Executes the command asynchronously using its current state.
        /// </summary>
        /// <returns>A <see cref="CommandResult"/> representing the outcome of the execution.</returns>
        Task<CommandResult> ExecuteAsync();

        /// <summary>
        /// Resets the command’s internal state to its initial condition.
        /// </summary>
        void Reset();
    }
}