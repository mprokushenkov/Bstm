using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DNTests : TestBase
    {
        public DNTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void PublicInterfaceThrowsExceptionForNullArguments()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(DN.Parse("CN=John"));
            var assertion = new GuardClauseAssertion(fixture);

            // Exercise system and verify outcome
            assertion.Verify(typeof(DN).GetConstructors());

            assertion.Verify(typeof(DN).GetMethod("Append", new[] { typeof(Rdn[]) } ));
            assertion.Verify(typeof(DN).GetMethod("Append", new[] { typeof(DN) } ));

            assertion.Verify(typeof(DN).GetMethod("Prepend", new[] { typeof(Rdn[]) }));
            assertion.Verify(typeof(DN).GetMethod("Prepend", new[] { typeof(DN) } ));
        }

        [Fact]
        public void ExceptionThrownForEmptyRdnList()
        {
            // Fixture setup
            // ReSharper disable once ObjectCreationAsStatement
            Action call = () => new DN();

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Supplied rdn list can not be empty.");
        }

        [Fact]
        public void ToStringReturnsCorrectDnStringRepresentationForSuppliedRdnList()
        {
            // Fixture setup
            var comRdn = new Rdn(NamingAttribute.Dc, new LdapName("com"));
            var domainRdn = new Rdn(NamingAttribute.Dc, new LdapName("domain"));
            var usersRdn = new Rdn(NamingAttribute.Ou, new LdapName("Users"));
            var johnRdn = new Rdn(NamingAttribute.Cn, new LdapName("John Doe"));

            var dn = new DN(johnRdn, usersRdn, domainRdn, comRdn);

            // Exercise system and verify outcome
            dn.ToString().Should().Be("CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void ToStringReturnsCorrectDnStringRepresentationForSuppliedRdn()
        {
            // Fixture setup
            var dn = new DN(new Rdn(NamingAttribute.Cn, new LdapName("John Doe")));

            // Exercise system and verify outcome
            dn.ToString().Should().Be("CN=John Doe");
        }

        [Fact]
        public void AppendRdnsReturnsUpdatedInstance()
        {
            // Fixture setup
            var dn = new DN(new Rdn(NamingAttribute.Cn, new LdapName("John Doe")));

            // Exercise system
            dn = dn.Append(new Rdn(NamingAttribute.Ou, new LdapName("Users")));

            // Verify outcome
            dn.ToString().Should().Be("CN=John Doe,OU=Users");
        }

        [Fact]
        public void AppendDNReturnsUpdatedInstance()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John Doe,OU=Users");

            // Exercise system
            dn = dn.Append(DN.Parse("DC=domain,DC=com"));

            // Verify outcome
            dn.ToString().Should().Be("CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void PrependReturnsUpdatedInstance()
        {
            // Fixture setup
            var dn = new DN(new Rdn(NamingAttribute.Ou, new LdapName("Users")));

            // Exercise system
            dn = dn.Prepend(new Rdn(NamingAttribute.Cn, new LdapName("John Doe")));

            // Verify outcome
            dn.ToString().Should().Be("CN=John Doe,OU=Users");
        }

        [Fact]
        public void PrependDNReturnsUpdatedInstance()
        {
            // Fixture setup
            var dn = DN.Parse("DC=domain,DC=com");

            // Exercise system
            dn = dn.Prepend(DN.Parse("CN=John Doe,OU=Users"));

            // Verify outcome
            dn.ToString().Should().Be("CN=John Doe,OU=Users,DC=domain,DC=com");
        }

        [Fact]
        public void DnIsImmutable()
        {
            // Fixture setup
            var sourceDn = new DN(new Rdn(NamingAttribute.Cn, new LdapName("John Doe")));

            // Exercise system
            var targetDn = sourceDn.Append(new Rdn(NamingAttribute.Ou, new LdapName("Users")));

            // Verify outcome
            targetDn.Should().NotBeSameAs(sourceDn);
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsCorrectDnRepresentation()
        {
            // Fixture setup
            string dn = new DN(new Rdn(NamingAttribute.Cn, new LdapName("John")));

            // Exercise system and verify outcome
            dn.Should().Be("CN=John");
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users,DC=domain,DC=com");
            var otherDn = DN.Parse("CN=John,OU=Users,DC=OMSK,DC=domain,DC=com");

            // ReSharper disable once ImplicitlyCapturedClosure
            Fixture.Inject(dn.Rdns.ToArray());

            var configuration = EqualityTestsConfigurer<DN>
                .Instance(dn)
                .ShouldBeEqualTo(dn)
                .ShouldNotBeEqualTo(otherDn);

            // Exercise system and verify outcome
            EqualityTestsFor<DN>.Assert(() => configuration, Fixture);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionThrownForInvalidValueOnParse(string value)
        {
            // Fixture setup
            Action call = () => DN.Parse(value);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("DN string can not be null or white space.");
        }

        [Fact]
        public void ParseCorrectDnStringReturnsCorrectDn()
        {
            // Fixture setup
            var dnString = @"CN=John\, Doe , OU=Users , DC=domain , DC=com";

            // Exercise system
            var dn = DN.Parse(dnString);

            // Verify outcome
            dn.ToString().Should().Be(@"CN=John\, Doe,OU=Users,DC=domain,DC=com");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionThrownForInvalidValueOnFromFqdn(string value)
        {
            // Fixture setup
            Action call = () => DN.FromFqdn(value);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Fqdn string can not be null or white space.");
        }

        [Fact]
        public void FromCorrectFqdnStringReturnsCorrectDn()
        {
            // Fixture setup
            var fqdnString = @"domain.com";

            // Exercise system
            var dn = DN.FromFqdn(fqdnString);

            // Verify outcome
            dn.ToString().Should().Be(@"DC=domain,DC=com");
        }

        [Fact]
        public void ExceptionThrownForCandidateWithoutEscaping()
        {
            // Fixture setup

            // Exercise system
            Action call = () => DN.Parse("CN=John, Doe,OU=Users,DC=domain,DC=com");

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage(
                    "String 'CN=John, Doe,OU=Users,DC=domain,DC=com' can not be converted to valid DN instance.");
        }

        [Fact]
        public void RdnsPropertyReturnsSameCollectionAsConstructedFrom()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users");

            var expected = new[]
            {
                new Rdn(NamingAttribute.Cn, new LdapName("John")),
                new Rdn(NamingAttribute.Ou, new LdapName("Users"))
            };

            // Exercise system and verify outcome
            // ReSharper disable once CoVariantArrayConversion
            dn.Rdns.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("OU=Users,CN=John")]
        [InlineData("DC=domain,OU=Users")]
        [InlineData("DC=domain,CN=John")]
        [InlineData("CN=John,DC=domain")]
        [InlineData("CN=John,OU=Users,DC=domain,CN=John")]
        public void RdnSequenceCorrectnessShouldBeChecked(string dn)
        {
            // Fixture setup
            Action call = () => DN.Parse(dn);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage($"Rdn sequence '{dn}' can not be converted to valid DN instance.");
        }

        [Theory]
        [InlineData("CN=John,CN=Users,DC=domain,DC=com")]
        [InlineData("CN=Access Control Assistance Operators,CN=Builtin,DC=bstm,DC=com")]
        [InlineData("CN=DC1,CN=Computers,DC=bstm,DC=com")]
        [InlineData("CN=S-1-5-9,CN=ForeignSecurityPrincipals,DC=bstm,DC=com")]
        [InlineData("CN=S-1-5-9,CN=LostAndFound,DC=bstm,DC=com")]
        [InlineData("CN=John,CN=Managed Service Accounts,DC=domain,DC=com")]
        [InlineData("CN=John,CN=Program Data,DC=domain,DC=com")]
        [InlineData("CN=John,CN=System,DC=domain,DC=com")]
        [InlineData("CN=John,CN=NTDS Quotas,DC=domain,DC=com")]
        [InlineData("CN=John,CN=TPM Devices,DC=domain,DC=com")]
        [InlineData("CN=John,CN=Infrastructure,DC=domain,DC=com")]
        public void DNIsValidIfObjectContainsInWellknownGenericContainer(string dn)
        {
            // Fixture setup
            Action call = () => DN.Parse(dn);

            // Exercise system and verify outcome
            call.Should().NotThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void AddressableObjectNameReturnsFirstRdn()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users");
            var expected = Rdn.Parse("CN=John");

            // Exercise system and verify outcome
            dn.AddressableObjectName.Should().Be(expected);
        }

        [Fact]
        public void ParentReturnsDnFromParentRdn()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users,DC=domain,DC=com");
            var expected = DN.Parse("OU=Users,DC=domain,DC=com");

            // Exercise system and verify outcome
            dn.Parent.Should().Be(expected);
        }

        [Fact]
        public void ParentReturnsIfNoParentRdn()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            // Exercise system and verify outcome
            dn.Parent.Should().BeNull();
        }

        [Fact]
        public void DomainReturnsDnFromDcRdns()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users,DC=domain,DC=com");
            var expected = DN.Parse("DC=domain,DC=com");

            // Exercise system and verify outcome
            dn.Domain.Should().Be(expected);
        }

        [Fact]
        public void FqdnReturnsCorrectString()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users,DC=domain,DC=com");

            // Exercise system and verify outcome
            dn.Fqdn.Should().Be("domain.com");
        }

        [Fact]
        public void DomainReturnsNullIfNoDcRdns()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John,OU=Users");

            // Exercise system and verify outcome
            dn.Domain.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsFalseOnInvalidValue()
        {
            // Fixture setup

            // Exercise system
            var success = DN.TryParse("Hello", out var result);

            // Verify outcome
            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsTrueOnCorrectValue()
        {
            // Fixture setup

            // Exercise system
            var success = DN.TryParse("CN=John,OU=Users,DC=domain,DC=com", out var result);

            // Verify outcome
            success.Should().BeTrue();
            result.ToString().Should().Be("CN=John,OU=Users,DC=domain,DC=com");
        }
    }
}