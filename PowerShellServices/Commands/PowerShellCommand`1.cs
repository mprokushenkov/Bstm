using System.Collections.Generic;
using System.Management.Automation;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices.Commands
{
    public abstract class PowerShellCommand<TResult> : PowerShellCommand
    {
        private readonly Dictionary<CommandParameter, object> parameters = new Dictionary<CommandParameter, object>();

        protected PowerShellCommand([NotNull] string name) : base(name)
        {
        }

        [NotNull]
        internal TResult CreateResult([NotNull] ICollection<PSObject> psObjects)
        {
            Guard.CheckNull(psObjects, nameof(psObjects));

            return CreateResultInternal(psObjects);
        }

        [NotNull]
        internal abstract TResult CreateResultInternal([NotNull]  ICollection<PSObject> psObjects);
    }
}