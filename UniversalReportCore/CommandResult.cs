using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public class CommandResult
    {
        public bool Success { get; }
        public string Message { get; }

        private CommandResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static CommandResult Ok(string message = "Command executed successfully")
            => new CommandResult(true, message);

        public static CommandResult Fail(string message)
            => new CommandResult(false, message);
    }
}
