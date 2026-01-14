using System;
using System.Text.Json;
using UniversalReportCore;
using UniversalReportCore.Helpers;

namespace UniversalReportCore
{
    /// <summary>
    /// Abstract base class for commands that process data of type <typeparamref name="T"/>.
    /// Provides a framework for deserializing JSON data, executing commands, and managing state.
    /// </summary>
    /// <typeparam name="T">The type of data the command processes.</typeparam>
    public abstract class CommandBase<T> : ICommand
    {
        /// <summary>
        /// The deserialized data value used by the command during execution.
        /// </summary>
        protected T _dataValue = default!;

        /// <summary>
        /// Deserializes a JSON string into the command’s data value.
        /// </summary>
        /// <param name="data">The JSON string to deserialize.</param>
        /// <returns>The deserialized data as an <see cref="object"/>.</returns>
        /// <exception cref="JsonException">Thrown if <paramref name="data"/> is null or empty.</exception>
        public object Deserialize(string data)
        {
            return DeserializeData(data)!;
        }

        /// <summary>
        /// Deserializes a JSON string into the command’s specific data type <typeparamref name="T"/> and sets the internal state.
        /// </summary>
        /// <param name="data">The JSON string to deserialize.</param>
        /// <returns>The deserialized data of type <typeparamref name="T"/>.</returns>
        /// <exception cref="JsonException">Thrown if <paramref name="data"/> is null, empty, or cannot be deserialized into <typeparamref name="T"/>.</exception>
        protected T DeserializeData(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new JsonException("Command data cannot be null or empty.");

            _dataValue = JsonHelper.Deserialize<T>(data); // Adjust to JsonHelper if needed
            return _dataValue;
        }

        /// <summary>
        /// Sets the command’s data value directly, bypassing JSON deserialization.
        /// </summary>
        /// <param name="data">The data value to set.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="data"/> is not of type <typeparamref name="T"/>.</exception>
        public void SetData(object data)
        {
            if (data is T typedData)
            {
                _dataValue = typedData;
            }
            else
            {
                throw new ArgumentException($"Data must be of type {typeof(T).Name}.", nameof(data));
            }
        }

        /// <summary>
        /// Determines whether the command can execute with its current data value.
        /// </summary>
        /// <returns><c>true</c> if the command can execute (i.e., <see cref="_dataValue"/> is not null); otherwise, <c>false</c>.</returns>
        /// <remarks>Derived classes can override this method to implement custom execution conditions.</remarks>
        public virtual bool CanExecute()
        {
            return _dataValue != null; // Basic check; derived classes can override
        }

        /// <summary>
        /// Executes the command asynchronously using the current data value.
        /// </summary>
        /// <returns>A <see cref="CommandResult"/> representing the outcome of the command execution.</returns>
        public abstract Task<CommandResult> ExecuteAsync();

        /// <summary>
        /// Resets the command’s state by clearing the data value.
        /// </summary>
        /// <remarks>Derived classes can override this method to reset additional state if needed.</remarks>
        public virtual void Reset()
        {
            _dataValue = default!; // Clears the stored data
        }
    }
}