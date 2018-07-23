using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices.Workers
{
    internal interface IPowerShellExecutor
    {
        [NotNull]
        ICollection<PSObject> Execute([NotNull] Command command);
    }
}