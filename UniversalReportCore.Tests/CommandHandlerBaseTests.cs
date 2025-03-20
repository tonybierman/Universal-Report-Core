using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UniversalReportCore;
using Xunit;

namespace UniversalReportCore.Tests
{
    public class CommandHandlerBaseTests
    {
        // Mock command for testing
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

        [Fact]
        public async Task ExecuteAsync_WithStringData_ValidCommand_Succeeds()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            string data = JsonSerializer.Serialize(42); // Valid int

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Executed command for ID 42", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_WithStringData_InvalidJson_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            string data = "invalid json"; // Invalid JSON

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Failed to deserialize data for command 'test'", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_WithStringData_CommandNotFound_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            string data = JsonSerializer.Serialize(42);

            // Act
            var result = await handler.ExecuteAsync("unknown", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Command 'unknown' not found.", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_WithStringData_CanExecuteFalse_Fails()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            string data = JsonSerializer.Serialize(0); // Invalid per CanExecute

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Command 'test' cannot be executed with the provided data.", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_WithStringData_ExecutionFails_ReturnsFailure()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand(id => Task.FromResult(false)) // Simulate failure
            };
            var handler = new CommandHandlerBase(commands);
            string data = JsonSerializer.Serialize(42);

            // Act
            var result = await handler.ExecuteAsync("test", data);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failed to execute command for ID 42", result.Message);
        }

        [Fact]
        public async Task ExecuteAsync_WithObjectData_ValidCommand_Succeeds_Debug()
        {
            // Arrange
            var command = new TestCommand();
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = command
            };
            var handler = new CommandHandlerBase(commands);
            string jsonData = JsonSerializer.Serialize(42); // "42"
            command.Deserialize(jsonData);

            // Debug
            //Assert.Equal(42, command._dataValue); // Should pass
            Assert.True(command.CanExecute()); // Should pass

            // Act
            var result = await handler.ExecuteAsync("test", (object)null); // Explicitly cast to object

            // Assert
            Assert.True(result.Success, $"Expected success but got: {result.Message}");
            Assert.Equal("Executed command for ID 42", result.Message);
        }

        [Fact]
        public void Deserialize_SetsDataValue_Correctly()
        {
            // Arrange
            var command = new TestCommand();
            string jsonData = JsonSerializer.Serialize(42); // "42"

            // Act
            command.Deserialize(jsonData);

            // Assert
            //Assert.Equal(42, command._dataValue);
            Assert.True(command.CanExecute());
        }

        [Fact]
        public async Task ExecuteAsync_WithObjectData_ValidCommand_Succeeds()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            var command = commands["test"];
            command.Deserialize(JsonSerializer.Serialize(42)); // Pre-set _dataValue

            // Act
            var result = await handler.ExecuteAsync("test", (object)null);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Executed command for ID 42", result.Message);
        }

        [Fact]
        public void CanExecute_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            var command = commands["test"];
            command.Deserialize(JsonSerializer.Serialize(42)); // Set _dataValue

            // Act
            bool canExecute = handler.CanExecute("test", null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void CanExecute_CommandNotFound_ReturnsFalse()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);

            // Act
            bool canExecute = handler.CanExecute("unknown", null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void CanExecute_CanExecuteFalse_ReturnsFalse()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            var command = commands["test"];
            command.Deserialize(JsonSerializer.Serialize(0)); // Invalid per CanExecute

            // Act
            bool canExecute = handler.CanExecute("test", null);

            // Assert
            Assert.False(canExecute);
        }


        [Fact]
        public async Task Reset_ClearsPreviousState()
        {
            // Arrange
            var commands = new Dictionary<string, ICommand>
            {
                ["test"] = new TestCommand()
            };
            var handler = new CommandHandlerBase(commands);
            var command = commands["test"];
            command.Deserialize(JsonSerializer.Serialize(42)); // Set _dataValue
            await handler.ExecuteAsync("test", JsonSerializer.Serialize(42)); // Execute once

            // Act: Reset and try with invalid data
            var result = await handler.ExecuteAsync("test", JsonSerializer.Serialize(0));

            // Assert: Should fail due to CanExecute after reset and new data
            Assert.False(result.Success);
            Assert.Equal("Command 'test' cannot be executed with the provided data.", result.Message);
        }


    }
}