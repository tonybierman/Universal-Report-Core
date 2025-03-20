using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public partial class CommandHandlerBaseIntTests
    {
        // Mock command for testing (from earlier tests)
        private class TestCommand : CommandBase<int>
        {
            private readonly Func<int, Task<bool>> _executeAction;

            public TestCommand(Func<int, Task<bool>> executeAction = null)
            {
                _executeAction = executeAction ?? (id => Task.FromResult(true));
            }

            public override bool CanExecute()
            {
                return _dataValue > 0; // Valid if ID is positive
            }

            public override async Task<CommandResult> ExecuteAsync()
            {
                if (await _executeAction(_dataValue))
                {
                    return CommandResult.Ok($"Executed command for ID {_dataValue}");
                }
                return CommandResult.Fail($"Failed to execute command for ID {_dataValue}");
            }
        }

        /// <summary>
        /// Tests that ExecuteAsync with an integer data parameter successfully executes a command.
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_WithIntData_ValidCommand_Succeeds()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            int data = 42;

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Executed command for ID 42", result.Message);
        }



        /// <summary>
        /// Tests that ExecuteAsync with an integer data parameter fails when CanExecute returns false.
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_WithIntData_CanExecuteFalse_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            int data = 0; // Fails CanExecute (_dataValue > 0)

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Command 'test' cannot be executed with data '0'.", result.Message);
        }

        /// <summary>
        /// Tests that ExecuteAsync with an integer data parameter fails when the command is not found.
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_WithIntData_CommandNotFound_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            int data = 42;

            // Act
            var result = await handler.ExecuteAsync("unknown", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Command 'unknown' not found.", result.Message);
        }

        /// <summary>
        /// Tests that ExecuteAsync with an integer data parameter fails when the command doesn’t support int.
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_WithIntData_NonIntCommand_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new NonIntTestCommand() // Concrete class instead of abstract
            };
            var handler = new CommandHandlerBase(commands);
            int data = 42;

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Command 'test' does not support integer data.", result.Message);
        }

        // Minimal concrete implementation for testing
        private class NonIntTestCommand : CommandBase<string>
        {
            public override Task<CommandResult> ExecuteAsync()
            {
                return Task.FromResult(CommandResult.Ok(""));
            }
        }
    }
}