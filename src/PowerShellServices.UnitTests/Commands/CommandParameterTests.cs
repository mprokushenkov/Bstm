using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Bstm.PowerShellServices.Commands.CommandParameter;

namespace Bstm.PowerShellServices.UnitTests.Commands
{
    public class CommandParameterTests : TestBase
    {
        public CommandParameterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void ParameterNamesDefinedCorrect()
        {
            // Fixture setup

            // Exercise system and verify outcome
            ComputerName.Name.Should().Be("ComputerName");
            ComputerName.ConvertFromString("string").As<string>().Should().Be("string");

            Command.Name.Should().Be("Command");
            Command.ConvertFromString("string").As<string>().Should().Be("string");
        }
    }
}