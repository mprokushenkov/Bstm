using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices.Commands
{
    public class PowerShellCommand
    {
        public PowerShellCommand([NotNull] string name)
        {
            Name = Guard.CheckNullOrEmpty(name, nameof(name));
        }

        public string Name { get; }

        internal Dictionary<CommandParameter, object> Parameters { get; } = new Dictionary<CommandParameter, object>();

        public T GetParameter<T>([NotNull] CommandParameter parameter)
        {
            Guard.CheckNull(parameter, nameof(parameter));

            if (Parameters.ContainsKey(parameter))
            {
                return (T) Parameters[parameter];
            }

            return default(T);
        }

        public void SetParameter([NotNull] CommandParameter parameter,object value)
        {
            Guard.CheckNull(parameter, nameof(parameter));

            Parameters[parameter] = value;
        }

        [NotNull]
        internal Command ToNativeCommand()
        {
            var command = new Command(Name);
            Parameters.ForEach(p => command.Parameters.Add(p.Key, p.Value));

            return command;
        }
    }
}