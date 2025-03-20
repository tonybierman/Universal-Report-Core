using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    /// <summary>
    /// Base class for handling commands by delegating to registered <see cref="ICommand"/> implementations.
    /// Provides multiple execution paths: pre-deserialized object, raw JSON strings, and integer data.
    /// </summary>
    public class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// Dictionary of command names mapped to their corresponding <see cref="ICommand"/> instances.
        /// </summary>
        protected readonly Dictionary<string, ICommand> _commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class with a dictionary of commands.
        /// </summary>
        /// <param name="commands">A dictionary mapping command names to their <see cref="ICommand"/> implementations.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commands"/> is null.</exception>
        public CommandHandlerBase(Dictionary<string, ICommand> commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        /// <summary>
        /// Determines whether a command can be executed with the given pre-deserialized data.
        /// This method is retained for compatibility with legacy flows but is not used in the primary execution path.
        /// </summary>
        /// <param name="commandName">The name of the command to check.</param>
        /// <param name="commandData">The pre-deserialized data for the command (ignored in current flow).</param>
        /// <returns><c>true</c> if the command exists and can execute; otherwise, <c>false</c>.</returns>
        public virtual bool CanExecute(string commandName, object commandData)
        {
            return _commands.TryGetValue(commandName, out var command) &&
                   command.CanExecute();
        }

        /// <summary>
        /// Executes a command asynchronously using pre-deserialized data.
        /// This method is retained for compatibility with legacy flows and relies on the command’s pre-set state.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="commandData">The pre-deserialized data for the command (optional, can be null if state is pre-set).</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is null.</exception>
        public virtual async Task<CommandResult> ExecuteAsync(string commandName, object commandData)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException(nameof(commandName));

            if (!_commands.TryGetValue(commandName, out var command))
            {
                return CommandResult.Fail($"Command '{commandName}' not found.");
            }

            if (!command.CanExecute())
            {
                return CommandResult.Fail($"Command '{commandName}' cannot be executed.");
            }

            return await command.ExecuteAsync();
        }

        /// <summary>
        /// Executes a command asynchronously by deserializing raw JSON data and resetting the command’s state.
        /// This is the primary execution path for commands that implement <see cref="CommandBase{T}"/>.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="data">The raw JSON string containing the command’s data.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is null.</exception>
        public virtual async Task<CommandResult> ExecuteAsync(string commandName, string data)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException(nameof(commandName));

            if (!_commands.TryGetValue(commandName, out var command))
            {
                return CommandResult.Fail($"Command '{commandName}' not found.");
            }

            try
            {
                command.Reset(); // Clear previous state
                command.Deserialize(data); // Sets _dataValue
            }
            catch (JsonException ex)
            {
                return CommandResult.Fail($"Failed to deserialize data for command '{commandName}': {ex.Message}");
            }

            if (!command.CanExecute())
            {
                return CommandResult.Fail($"Command '{commandName}' cannot be executed with the provided data.");
            }

            return await command.ExecuteAsync();
        }

        /// <summary>
        /// Executes a command asynchronously using an integer as the command data.
        /// Resets the command’s state and sets the data directly, bypassing JSON deserialization.
        /// </summary>
        /// <param name="commandName">The name of the command to execute.</param>
        /// <param name="data">The integer data for the command.</param>
        /// <returns>A <see cref="CommandResult"/> indicating the outcome of the command execution.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the command does not support integer data (i.e., is not a <see cref="CommandBase{T}"/> with T as <see cref="int"/>).</exception>
        public virtual async Task<CommandResult> ExecuteAsync(string commandName, int data)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException(nameof(commandName));

            if (!_commands.TryGetValue(commandName, out var command))
            {
                return CommandResult.Fail($"Command '{commandName}' not found.");
            }

            // Ensure the command is a CommandBase<int> to set _dataValue via SetData
            if (command is CommandBase<int> intCommand)
            {
                intCommand.Reset(); // Clear previous state
                intCommand.SetData(data); // Set _dataValue via protected method

                if (!intCommand.CanExecute())
                {
                    return CommandResult.Fail($"Command '{commandName}' cannot be executed with data '{data}'.");
                }

                return await intCommand.ExecuteAsync();
            }

            return CommandResult.Fail($"Command '{commandName}' does not support integer data.");
        }
    }
}