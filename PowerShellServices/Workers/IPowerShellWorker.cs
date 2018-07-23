using System.Threading.Tasks;
using Bstm.PowerShellServices.Commands;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices.Workers
{
    public interface IPowerShellWorker
    {
        void RunCommand([NotNull] PowerShellCommand command);

        TResult RunCommand<TResult>([NotNull] PowerShellCommand<TResult> command);

        Task<TResult> RunCommandAsync<TResult>([NotNull] PowerShellCommand<TResult> command);

        Task RunCommandAsync([NotNull] PowerShellCommand command);
    }
}