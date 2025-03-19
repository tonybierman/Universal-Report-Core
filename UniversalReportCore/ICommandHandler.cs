using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public interface ICommandHandler
    {
        bool CanExecute(string commandName, object commandData);
        Task<CommandResult> ExecuteAsync(string commandName, object commandData);
        Task<CommandResult> ExecuteAsync(string commandName, string commandData);
    }
}
