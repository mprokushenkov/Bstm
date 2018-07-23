using AutoFixture.Idioms;
using Bstm.DirectoryServices;
using Bstm.UnitTesting;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class GroupMembersCollectionTests : TestBase
    {
        public GroupMembersCollectionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(GroupMembersCollection).GetConstructors());

            assertion.Verify(
                typeof(GroupMembersCollection).GetMethods());
        }
    }
}