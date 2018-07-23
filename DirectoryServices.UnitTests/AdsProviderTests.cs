using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class AdsProviderTests : TestBase
    {
        public AdsProviderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void ProviderNamesCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            AdsProvider.Ldap.Name.Should().Be("LDAP");
            AdsProvider.GC.Name.Should().Be("GC");
        }
    }
}