using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmaTools.Models
{
    class ProcessCollecter
    {
        public List<(string ProcessName, int ProcessId)> collect()
        {
            // Get the current process.
            Process currentProcess = Process.GetCurrentProcess();

            // Get all processes running on the local computer.
            Process[] localAll = Process.GetProcesses();

            // Get all instances of Notepad running on the local computer.
            // This will return an empty array if notepad isn't running.
            Process[] localByName = Process.GetProcessesByName("notepad");
            
            return localAll.Where(x => !string.IsNullOrEmpty(x.MainWindowTitle)).Select(x => (x.ProcessName, x.Id)).ToList();
        }
    }
}
