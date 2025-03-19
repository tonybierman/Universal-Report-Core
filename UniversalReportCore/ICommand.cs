using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore
{
    public interface ICommand
    {
        object Deserialize(string data);
        bool CanExecute();
        Task<CommandResult> ExecuteAsync();
        void Reset();
    }
}
