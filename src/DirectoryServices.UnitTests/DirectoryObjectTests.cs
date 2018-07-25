using System.DirectoryServices;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class DirectoryObjectTests : TestBase
    {
        public DirectoryObjectTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Inject(new DirectoryEntry());
        }

        [Fact]
        public void PublicInterfaceThrowsExceptionForNullArguments()
        {
            // Fixture setup
            var assertion = new GuardClauseAssertion(Fixture);

            // Exercise system and verify outcome
            assertion.Verify(typeof(DirectoryObject));
        }

        [Fact]
        public void DisplayNameShouldBeStored()
        {
            // Fixture setup
            var displayName = Fixture.Create<string>();
            var directoryObject = Fixture.Create<DirectoryObject>();

            // Exercise system
            directoryObject.DisplayName = displayName;

            // Verify outcome
            directoryObject.DisplayName.Should().Be(displayName);
            directoryObject.GetPropertyValue<string>(DirectoryProperty.DisplayName).Should().Be(displayName);
        }

        [Fact]
        public void NameShouldBeCorrectComputed()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");
            var directoryObject = Fixture.Create<DirectoryObject>();

            // Exercise system
            directoryObject.SetPropertyValue(DirectoryProperty.DistinguishedName, dn);

            // Verify outcome
            directoryObject.Name.Should().Be("John");
        }

        [Fact]
        public void DistinguishedNameShouldHaveCorrectGetter()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");
            var directoryObject = Fixture.Create<DirectoryObject>();
            directoryObject.SetPropertyValue(DirectoryProperty.DistinguishedName, dn);

            // Exercise system and verify outcome
            directoryObject.DistinguishedName.Should().Be(dn);
        }
    }
}