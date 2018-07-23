using System.Collections.ObjectModel;
using System.Management.Automation;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.PowerShellServices.Commands;
using Bstm.PowerShellServices.Workers;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.PowerShellServices.UnitTests.Workers
{
    public class PowerShellWorkerTests : TestBase
    {
        public PowerShellWorkerTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(PowerShellWorker));
        }

        [Theory]
        [AutoMoqData]
        public async void ExecutorShouldBeInvokedOnRunCommand(
            PowerShellCommand command)
        {
            // Fixture setup
            var executor = Fixture.Create<IPowerShellExecutor>();
            var worker = new PowerShellWorker(executor);

            // Exercise system
            await worker.RunCommandAsync(command);

            // Verify outcome
            executor.Verify(e => e.Execute(command.ToNativeCommand().AsLikeness()));
        }

        [Theory]
        [AutoMoqData]
        public void ExecutorShouldBeInvokedOnRunCommandWithResult(
            PowerShellCommand<string> command)
        {
            // Fixture setup
            var executor = Fixture.Create<IPowerShellExecutor>();
            var worker = new PowerShellWorker(executor);

            // Exercise system
            worker.RunCommand(command);

            // Verify outcome
            executor.Verify(e => e.Execute(command.ToNativeCommand().AsLikeness()));
        }

        [Theory]
        [AutoMoqData]
        public async void WorkerShouldReturnCommandResult(
            PowerShellCommand<string> command,
            Collection<PSObject> psObjects)
        {
            // Fixture setup
            var executor = Fixture.Create<IPowerShellExecutor>();
            var worker = new PowerShellWorker(executor);

            executor
                .SetupIgnoreArgs(e => e.Execute(null))
                .Returns(psObjects);

            command
                .SetupIgnoreArgs(c => c.CreateResultInternal(null))
                .Returns("Hello!");

            // Exercise system
            var result = await worker.RunCommandAsync(command);

            // Verify outcome
            result.Should().Be("Hello!");
        }
    }
}