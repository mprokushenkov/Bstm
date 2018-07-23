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
            DirectoryServices.SchemaClassName.Group.ToString().Should().Be("group");
            DirectoryServices.SchemaClassName.User.ToString().Should().Be("user");
            DirectoryServices.SchemaClassName.OrganizationalUnit.ToString().Should().Be("organizationalUnit");
        }
    }
}