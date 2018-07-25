using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class SchemaClassNameProviderTests : TestBase
    {
        public SchemaClassNameProviderTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void NamesCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            SchemaClassName.Group.ToString().Should().Be("group");
            SchemaClassName.User.ToString().Should().Be("user");
            SchemaClassName.OrganizationalUnit.ToString().Should().Be("organizationalUnit");
        }
    }
}