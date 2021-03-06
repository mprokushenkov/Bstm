﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Bstm.Common;
using NLog;

namespace Bstm.PowerShellServices.Workers
{
    internal class PowerShellExecutor : IPowerShellExecutor
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string RemoteAddress { get; set; }

        public string ShellName { get; set; }

        public ICollection<string> SnapIns { get; } = new Collection<string>();

        public ICollection<string> Modules { get; } = new Collection<string>();

        public ICollection<Command> StartupCommands { get; } = new Collection<Command>();

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public ICollection<PSObject> Execute(Command command)
        {
            Guard.CheckNull(command, nameof(command));

            using (var runspace = CreateRunspace())
            {
                logger.Debug("Create powershell.");

                using (var shell = PowerShell.Create())
                {
                    shell.Runspace = runspace;

                    InvokeCommands(shell, StartupCommands.ToArray());

                    var result = InvokeCommands(shell, command);

                    return result;
                }
            }
        }

        private Runspace CreateRunspace()
        {
            Runspace runspace;

            if (!string.IsNullOrEmpty(RemoteAddress))
            {
                var connectionInfo = GetConnectionInfo();

                logger.Debug("Create remote runspace");

                runspace = RunspaceFactory.CreateRunspace(connectionInfo);
            }
            else
            {
                logger.Debug("Create local runspace");
                runspace = RunspaceFactory.CreateRunspace(CreateSession());
            }

            var activityId = Trace.CorrelationManager.ActivityId;

            runspace.Open();

            // Propogate activityId because runspace.Open() creates new one.
            Trace.CorrelationManager.ActivityId = activityId;

            // set error action preference
            logger.Debug("Set variable 'ErrorActionPreference' as 'Continue'.");

            runspace.SessionStateProxy.SetVariable("ErrorActionPreference", "Continue");

            return runspace;
        }

        private InitialSessionState CreateSession()
        {
            logger.Debug("Initialize default session state.");
            var session = InitialSessionState.CreateDefault();

            logger.Debug("Import modules {@Modules}.", Modules);
            session.ImportPSModule(Modules.ToArray());

            logger.Debug("Import snapins {@SnapIns}.", SnapIns);
            SnapIns.ForEach(s => session.ImportPSSnapIn(s, out var _));

            return session;
        }

        private static ICollection<PSObject> InvokeCommands(PowerShell shell, params Command[] commands)
        {
            var results = new List<PSObject>();

            commands.ForEach(command =>
            {
                shell.Commands.AddCommand(command);

                logger.Debug(
                    "Invoke command {Command} with parameters {@Parameters}.",
                    command.CommandText,
                    command.Parameters);

                results.AddRange(shell.Invoke());

                shell.Commands.Clear();

                CheckErrors(shell.Streams.Error);

                logger.Debug("Command {Command} invoked successfully.", command.CommandText);
            });

            return results;
        }

        private WSManConnectionInfo GetConnectionInfo()
        {
            logger.Debug("Create connection info. Uri {ConnectionUri}, Shell {ShellName}.", RemoteAddress, ShellName);

            var connectionInfo = Uri.TryCreate(RemoteAddress, UriKind.Absolute, out var uri)
                ? new WSManConnectionInfo(uri) {ShellUri = ShellName}
                : new WSManConnectionInfo {ComputerName = RemoteAddress, ShellUri = ShellName};

            return connectionInfo;
        }

        private static void CheckErrors(ICollection<ErrorRecord> errors)
        {
            if (errors.Count == 0)
            {
                return;
            }

            var message = string.Join(
                Environment.NewLine,
                errors.Select(e => e.ErrorDetails?.Message ?? e.Exception.Message));

            var exception = errors.Count > 1
                ? new AggregateException(errors.Select(e => e.Exception))
                : errors.First().Exception;

            throw new PowerShellServicesException(message, exception);
        }
    }
}