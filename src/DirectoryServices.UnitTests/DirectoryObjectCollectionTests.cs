using AutoFixture.Idioms;
using Bstm.UnitTesting;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class DirectoryObjectCollectionTests : TestBase
    {
        public DirectoryObjectCollectionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(DirectoryObjectCollection));
        }
    }
}