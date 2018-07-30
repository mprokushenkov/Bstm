using System;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class AdsPathTests : TestBase
    {
        private readonly DN objectName = DN.Parse("CN=John Doe,OU=Users,DC=domain,DC=com");

        public AdsPathTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void CtorThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AdsPath).GetConstructors());
        }

        [Fact]
        public void CtorWithObjectPathOnlyReturnsAdsPathWithLdapServerLessBinding()
        {
            // Fixture setup
            var adsPath = new AdsPath(objectName);

            // Exercise system and verify outcome
            adsPath.ToString().Should().Be("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void CtorWithObjectPathAndServerReturnsAdsPathWithLdapServerFullBinding()
        {
            // Fixture setup
            var adsPath = new AdsPath("dc1.domain.com", objectName);

            // Exercise system and verify outcome
            adsPath.ToString().Should().Be("LDAP://dc1.domain.com/CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void CtorWithAllParametersReturnsCorrectAdsPath()
        {
            // Fixture setup
            var adsPath = new AdsPath(AdsProvider.GC, "dc1.domain.com", objectName);

            // Exercise system and verify outcome
            adsPath.ToString().Should().Be("GC://dc1.domain.com/CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void ParsePathWithServerReturnsCorrectAdsPath()
        {
            // Fixture setup
            var path = "LDAP://dc1.domain.com/CN=John Doe,OU=Users,DC=domain,DC=com";

            // Exercise system
            var adsPath = AdsPath.Parse(path);

            // Verify outcome
            adsPath.Provider.Should().Be(AdsProvider.Ldap);
            adsPath.Server.Should().Be("dc1.domain.com");
            adsPath.ObjectName.ToString().Should().Be("CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void ParsePathWithGuidNameReturnsCorrectAdsPath()
        {
            // Fixture setup
            var path = "LDAP://<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>";

            // Exercise system
            var adsPath = AdsPath.Parse(path);

            // Verify outcome
            adsPath.Provider.Should().Be(AdsProvider.Ldap);
            adsPath.ObjectName.ToString().Should().Be("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");
        }

        [Fact]
        public void ParsePathWithSidNameReturnsCorrectAdsPath()
        {
            // Fixture setup
            var path = "LDAP://<SID=S-1-5-32-544>";

            // Exercise system
            var adsPath = AdsPath.Parse(path);

            // Verify outcome
            adsPath.Provider.Should().Be(AdsProvider.Ldap);
            adsPath.ObjectName.ToString().Should().Be("<SID=S-1-5-32-544>");
        }

        [Fact]
        public void ParsePathWithoutServerReturnsCorrectAdsPath()
        {
            // Fixture setup
            const string path = "LDAP://CN=John Doe,OU=Users,DC=domain,DC=com";

            // Exercise system
            var adsPath = AdsPath.Parse(path);

            // Verify outcome
            adsPath.ToString().Should().Be("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void ParsePathWithoutProviderAndServerReturnsCorrectAdsPath()
        {
            // Fixture setup
            const string path = "CN=John Doe,OU=Users,DC=domain,DC=com";

            // Exercise system
            var adsPath = AdsPath.Parse(path);

            // Verify outcome
            adsPath.ToString().Should().Be("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void TryParseReturnsFalseOnInvalidValue()
        {
            // Fixture setup

            // Exercise system
            var success = AdsPath.TryParse("Hello", out var result);

            // Verify outcome
            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsTrueOnCorrectValue()
        {
            // Fixture setup

            // Exercise system
            var success = AdsPath.TryParse("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com", out var result);

            // Verify outcome
            success.Should().BeTrue();
            result.ToString().Should().Be("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionThrownForWhiteSpacePathOnParse(string path)
        {
            // Fixture setup
            Action call = () => AdsPath.Parse(path);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage($"String '{path}' can not be converted to valid AdsPath instance.");
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup
            var adsPath = new AdsPath(objectName);
            var otherPath = new AdsPath(new GuidName(Guid.Empty));

            Fixture.Inject(adsPath);
            Fixture.Inject(objectName);

            var configuration = EqualityTestsConfigurer<AdsPath>
                .Instance(adsPath)
                .ShouldBeEqualTo(adsPath)
                .ShouldNotBeEqualTo(otherPath);

            // Exercise system and verify outcome
            EqualityTestsFor<AdsPath>.Assert(() => configuration, Fixture);
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsCorrectAdsPathRepresentation()
        {
            // Fixture setup
            string actual = new AdsPath(objectName);

            // Exercise system and verify outcome
            actual.Should().Be("LDAP://CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void RootDseShoulBeCorrectPath()
        {
            // Fixture setup

            // Exercise system and verify outcome
            AdsPath.RootDse.ToString().Should().Be("LDAP://RootDSE");
        }
    }
}