using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Defines the contract for a command handler that manages the execution of commands.
    /// Supports pre-deserialized data, raw JSON strings, and integer data inputs.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Determines whether a command can be executed with the specified pre-deserialized data.
        /// </summary>
        /// <param name="commandName">The name of the command to check.</param>
        /// <param name="commandData">The pre-deserialized data for the command.</param>
        /// <returns><c>true</c> if the command exists and can execute; otherwise, <c>false</c>.</returns>
        bool CanExecute(string commandName, object commandData);

        /// <summary>
        /// Executes a command asynchronously using pre-deserialized data.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="commandData">The pre-deserialized data for the command.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        Task<CommandResult> ExecuteAsync(string commandName, object commandData);

        /// <summary>
        /// Executes a command asynchronously by deserializing raw JSON data.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="commandData">The raw JSON string containing the command’s data.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        Task<CommandResult> ExecuteAsync(string commandName, string commandData);

        /// <summary>
        /// Executes a command asynchronously using an integer as the command data.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="commandData">The integer data for the command.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        Task<CommandResult> ExecuteAsync(string commandName, int commandData);
    }
}