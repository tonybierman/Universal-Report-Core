using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public class CommandHandlerBase : ICommandHandler
    {
        protected readonly Dictionary<string, ICommand> _commands;

        public CommandHandlerBase(Dictionary<string, ICommand> commands) 
        { 
            _commands = commands; 
        }

        public virtual bool CanExecute(string commandName, object commandData)
        {
            // Kept for compatibility with existing calls, but not used in new flow
            return _commands.TryGetValue(commandName, out var command) &&
                   command.CanExecute();
        }

        public virtual async Task<CommandResult> ExecuteAsync(string commandName, object commandData)
        {
            // Kept for compatibility, but not used in new flow
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

        public virtual async Task<CommandResult> ExecuteAsync(string commandName, string data)
        {
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
    }
}
