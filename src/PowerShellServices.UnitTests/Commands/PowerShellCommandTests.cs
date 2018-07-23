using System.Management.Automation.Runspaces;
using AutoFixture.Idioms;
using Bstm.PowerShellServices.Commands;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using CommandParameter = System.Management.Automation.Runspaces.CommandParameter;

namespace Bstm.PowerShellServices.UnitTests.Commands
{
    public class PowerShellCommandTests : TestBase
    {
        public PowerShellCommandTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(GetProcessCommand).GetConstructors());
        }

        [Theory]
        [AutoMoqData]
        public void ToNativeCommandShouldReturnCorrectNativeCommand(GetProcessCommand command)
        {
            // Fixture setup
            var expectedParameters = new CommandParameterCollection()
            {
                new CommandParameter("ComputerName", command.ComputerName)
            };

            // Exercise system
            var nativeCommand = command.ToNativeCommand();

            // Verify outcome
            nativeCommand.Should().NotBeNull();
            nativeCommand.CommandText.Should().Be(command.Name);
            nativeCommand.Parameters.Should().BeEquivalentTo(expectedParameters);
        }
    }
}