using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Bstm.PowerShellServices.Commands
{
    public class GetProcessCommand : PowerShellCommand<IEnumerable<string>>
    {
        public GetProcessCommand() : base("Get-Process")
        {
        }

        public string ComputerName
        {
            get => GetParameter<string>(CommandParameter.ComputerName);
            set => SetParameter(CommandParameter.ComputerName, value);
        }

        internal override IEnumerable<string> CreateResultInternal(ICollection<PSObject> psObjects)
        {
            var result = new List<string>();
            result.AddRange(psObjects.Select(o => (string) o.Properties["Name"].Value).Distinct().OrderBy(s => s));

            return result;
        }
    }
}