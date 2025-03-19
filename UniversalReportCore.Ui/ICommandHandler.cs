using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui
{
    public interface ICommandHandler
    {
        bool CanExecute(string commandName, object commandData);
        Task ExecuteAsync(string commandName, object commandData);
    }
}
