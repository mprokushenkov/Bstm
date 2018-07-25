using AutoFixture.Idioms;
using Bstm.UnitTesting;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class GroupMemberCollectionTests : TestBase
    {
        public GroupMemberCollectionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(GroupMemberCollection).GetConstructors());

            assertion.Verify(
                typeof(GroupMemberCollection).GetMethods());
        }
    }
}