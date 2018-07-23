using System.Threading.Tasks;
using Bstm.Common;
using Bstm.PowerShellServices.Commands;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices.Workers
{
    internal class PowerShellWorker : IPowerShellWorker
    {
        public PowerShellWorker([NotNull] IPowerShellExecutor executor)
        {
            Executor = Guard.CheckNull(executor, nameof(executor));
        }

        internal IPowerShellExecutor Executor { get; }

        public void RunCommand(PowerShellCommand command)
        {
            Guard.CheckNull(command, nameof(command));
            Executor.Execute(command.ToNativeCommand());
        }

        [NotNull]
        public TResult RunCommand<TResult>(PowerShellCommand<TResult> command)
        {
            Guard.CheckNull(command, nameof(command));

            var psObjects = Executor.Execute(command.ToNativeCommand());
            var result = command.CreateResult(psObjects);

            return result;
        }

        public Task RunCommandAsync(PowerShellCommand command)
        {
            Guard.CheckNull(command, nameof(command));
            return Task.Run(() => RunCommand(command));
        }

        [NotNull]
        public Task<TResult> RunCommandAsync<TResult>(PowerShellCommand<TResult> command)
        {
            Guard.CheckNull(command, nameof(command));
            return Task.Run(() => RunCommand(command));
        }
    }
}