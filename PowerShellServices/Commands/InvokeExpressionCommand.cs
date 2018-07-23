using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Bstm.PowerShellServices.Commands
{
    public class InvokeExpressionCommand : PowerShellCommand<IEnumerable<string>>
    {
        public InvokeExpressionCommand() : base("Invoke-Expression")
        {
        }

        public string Command
        {
            get => GetParameter<string>(CommandParameter.Command);
            set => SetParameter(CommandParameter.Command, value);
        }

        internal override IEnumerable<string> CreateResultInternal(ICollection<PSObject> psObjects)
        {
            var result = new List<string>();
            result.AddRange(psObjects.Select(o => o.ToString()));

            return result;
        }
    }
}