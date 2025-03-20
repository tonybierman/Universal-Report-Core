using System;

namespace UniversalReportCore
{
    /// <summary>
    /// Represents the outcome of a command execution, indicating success or failure along with a descriptive message.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Gets a value indicating whether the command executed successfully.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// Gets the message describing the result of the command execution.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the command succeeded.</param>
        /// <param name="message">A message describing the result.</param>
        /// <remarks>This constructor is private to enforce the use of factory methods <see cref="Ok"/> and <see cref="Fail"/>.</remarks>
        private CommandResult(bool success, string message)
        {
            Success = success;
            Message = message ?? string.Empty; // Ensure Message is never null
        }

        /// <summary>
        /// Creates a successful <see cref="CommandResult"/> with an optional message.
        /// </summary>
        /// <param name="message">The success message. Defaults to "Command executed successfully" if not provided.</param>
        /// <returns>A new <see cref="CommandResult"/> instance representing a successful execution.</returns>
        public static CommandResult Ok(string message = "Command executed successfully")
            => new CommandResult(true, message);

        /// <summary>
        /// Creates a failed <see cref="CommandResult"/> with a specified message.
        /// </summary>
        /// <param name="message">The failure message describing why the command failed.</param>
        /// <returns>A new <see cref="CommandResult"/> instance representing a failed execution.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
        public static CommandResult Fail(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message), "Failure message cannot be null.");
            return new CommandResult(false, message);
        }
    }
}