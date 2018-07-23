using System.DirectoryServices;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.DirectoryServices;
using Bstm.UnitTesting;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class OrganizationalUnitTests : TestBase
    {
        public OrganizationalUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Inject(new DirectoryEntry());
        }

        [Fact]
        public void PublicInterfaceThrowsExceptionForNullArguments()
        {
            // Fixture setup
            var assertion = new GuardClauseAssertion(Fixture, new NullReferenceBehaviorExpectation());

            // Exercise system and verify outcome
            assertion.Verify(typeof(OrganizationalUnit));
        }
    }
}