using System;
using System.Text.Json;
using UniversalReportCore;
using UniversalReportCore.Helpers;

namespace UniversalReportCore
{
    public abstract class CommandBase<T> : ICommand
    {
        protected T _dataValue;

        public object Deserialize(string data)
        {
            return DeserializeData(data);
        }

        protected T DeserializeData(string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new JsonException("Command data cannot be null or empty.");

            _dataValue = JsonHelper.Deserialize<T>(data); // Assuming JsonUtility, adjust if using JsonHelper
            return _dataValue;
        }

        public virtual bool CanExecute()
        {
            return _dataValue != null; // Basic check; derived classes can override
        }

        public abstract Task<CommandResult> ExecuteAsync();

        // Reset the command's state
        public virtual void Reset()
        {
            _dataValue = default; // Clears the stored data
        }
    }
}