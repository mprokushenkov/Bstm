using Bstm.PowerShellServices.Commands;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.PowerShellServices.UnitTests.Commands
{
    public class PsObjectPropertyTests : TestBase
    {
        public PsObjectPropertyTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void PropertyNamesDefinedCorrect()
        {
            // Fixture setup

            // Exercise system and verify outcome
            PsObjectProperty.Alias.Name.Should().Be("Alias");
            PsObjectProperty.Database.Name.Should().Be("Database");
            PsObjectProperty.UseDatabaseQuotaDefaults.Name.Should().Be("UseDatabaseQuotaDefaults");
            PsObjectProperty.ProhibitSendQuota.Name.Should().Be("ProhibitSendQuota");
            PsObjectProperty.ProhibitSendReceiveQuota.Name.Should().Be("ProhibitSendReceiveQuota");
        }
    }
}