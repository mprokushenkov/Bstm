using System.Linq;
using AutoFixture.Idioms;
using Bstm.PowerShellServices.Commands;
using Bstm.PowerShellServices.Workers;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.PowerShellServices.UnitTests.Workers
{
    public sealed class PowerShellWorkerFactoryTests : TestBase
    {
        public PowerShellWorkerFactoryTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(PowerShellWorkerFactory));
        }

        [Fact]
        public void FactoryShouldInitializeRemoteAddress()
        {
            // Fixture setup
            var settings = new PowerShellWorkerSettings {RemoteAddress = "http://ya.ru"};

            // Exercise system
            var worker = (PowerShellWorker) PowerShellWorkerFactory.Create(settings);
            var executor = (PowerShellExecutor) worker.Executor;

            // Verify outcome
            executor.RemoteAddress.Should().Be(settings.RemoteAddress);
        }

        [Fact]
        public void FactoryShouldInitializeShellName()
        {
            // Fixture setup
            var settings = new PowerShellWorkerSettings {ShellName = "Microsoft.Exchange"};

            // Exercise system
            var worker = (PowerShellWorker) PowerShellWorkerFactory.Create(settings);
            var executor = (PowerShellExecutor) worker.Executor;

            // Verify outcome
            executor.ShellName.Should().Be(settings.ShellName);
        }

        [Fact]
        public void FactoryShouldInitializeModules()
        {
            // Fixture setup
            var settings = new PowerShellWorkerSettings();
            settings.Modules.Add("Module1");
            settings.Modules.Add("Module2");

            // Exercise system
            var worker = (PowerShellWorker) PowerShellWorkerFactory.Create(settings);
            var executor = (PowerShellExecutor) worker.Executor;

            // Verify outcome
            executor.Modules.Should().BeEquivalentTo(settings.Modules);
        }

        [Fact]
        public void FactoryShouldInitializeSnapIns()
        {
            // Fixture setup
            var settings = new PowerShellWorkerSettings();
            settings.SnapIns.Add("SnapIn1");
            settings.SnapIns.Add("SnapIn2");

            // Exercise system
            var worker = (PowerShellWorker) PowerShellWorkerFactory.Create(settings);
            var executor = (PowerShellExecutor) worker.Executor;

            // Verify outcome
            executor.SnapIns.Should().BeEquivalentTo(settings.SnapIns);
        }

        [Fact]
        public void FactoryShouldInitializeStartupCommands()
        {
            // Fixture setup
            var settings = new PowerShellWorkerSettings();
            settings.StartupCommands.Add(new GetProcessCommand());

            // Exercise system
            var worker = (PowerShellWorker) PowerShellWorkerFactory.Create(settings);
            var executor = (PowerShellExecutor) worker.Executor;

            // Verify outcome
            executor.StartupCommands.Should().BeEquivalentTo(settings.StartupCommands.Select(c => c.ToNativeCommand()));
        }
    }
}