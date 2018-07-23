using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bstm.PowerShellServices.Commands;

namespace Bstm.PowerShellServices
{
    public class PowerShellWorkerSettings
    {
        public string ShellName { get; set; }

        public ICollection<string> SnapIns { get; } = new Collection<string>();

        public ICollection<string> Modules { get; } = new Collection<string>();

        public ICollection<PowerShellCommand> StartupCommands { get; } = new Collection<PowerShellCommand>();

        public string RemoteAddress { get; set; }
    }
}