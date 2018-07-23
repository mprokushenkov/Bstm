using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class NamingAttributeTests : TestBase
    {
        public NamingAttributeTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void NamingAttributeHasCorrectDefinedNames()
        {
            // Fixture setup

            // Exercise system and verify outcome
            NamingAttribute.Cn.Name.Should().Be("CN");
            NamingAttribute.Ou.Name.Should().Be("OU");
            NamingAttribute.Dc.Name.Should().Be("DC");
        }
    }
}