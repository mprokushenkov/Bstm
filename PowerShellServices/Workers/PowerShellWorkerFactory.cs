using Bstm.Common;
using JetBrains.Annotations;
using NLog;

namespace Bstm.PowerShellServices.Workers
{
    public static class PowerShellWorkerFactory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [NotNull]
        public static IPowerShellWorker Create([NotNull] PowerShellWorkerSettings settings)
        {
            Guard.CheckNull(settings, nameof(settings));

            logger.Debug("Create PowerShell worker with following settings {@Settings}.", settings);

            var executor = new PowerShellExecutor();
            executor.RemoteAddress = settings.RemoteAddress;
            executor.ShellName = settings.ShellName;
            settings.Modules.ForEach(m => executor.Modules.Add(m));
            settings.SnapIns.ForEach(s => executor.SnapIns.Add(s));
            settings.StartupCommands.ForEach(c => executor.StartupCommands.Add(c.ToNativeCommand()));

            var worker = new PowerShellWorker(executor);

            return worker;
        }
    }
}