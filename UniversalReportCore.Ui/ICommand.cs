using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalReportCore.Ui
{
    public interface ICommand
    {
        bool CanExecute(object commandData);
        Task ExecuteAsync(object commandData);
    }
}
