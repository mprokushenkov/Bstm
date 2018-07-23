using System;
using System.Text;
using Bstm.Common;
using Bstm.PowerShellServices.Commands;
using Bstm.PowerShellServices.Workers;
using Bstm.UnitTesting;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.PowerShellServices.IntegrationTests.Workers
{
    public sealed class PowerShellWorkerFactoryTests : TestBase
    {
        public PowerShellWorkerFactoryTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact(Skip = "Integration test")]
        public void FactoryWorkerTest()
        {
            // Fixture setup
            var command = new GetProcessCommand
            {
                ComputerName = Environment.MachineName
            };

            var settings = new PowerShellWorkerSettings {RemoteAddress = Environment.MachineName};

            var worker = PowerShellWorkerFactory.Create(settings);

            // Exercise system
            var results = worker.RunCommand(command);

            var sb = new StringBuilder();
            results.ForEach(r => sb.AppendLine(r));

            // Verify outcome
            WriteMessage(sb.ToString());
        }
    }
}