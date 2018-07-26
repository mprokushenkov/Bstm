using System;
using System.Linq;
using ActiveDs;
using AutoFixture.Idioms;
using Bstm.Common;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Bstm.DirectoryServices.DirectoryProperty;
using static Bstm.DirectoryServices.DirectoryPropertySyntax;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DirectoryPropertyTests : TestBase
    {
        public DirectoryPropertyTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup
            var excluded = new[]
            {
                nameof(DirectoryProperty.ConvertFromDirectoryValue),
                nameof(Equals),
                nameof(GetHashCode)
            };

            // Exercise system and verify outcome
            assertion.Verify(typeof(DirectoryProperty).GetMethods().Where(m => !excluded.Contains(m.Name)));
        }

        [Fact]
        public void PropertiesCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            CheckPropertyCorrectDefined(Member, "member", DNString, true, typeof(DN), typeof(string));
            CheckPropertyCorrectDefined(MemberOf, "memberOf", DNString, true, typeof(DN), typeof(string));
            CheckPropertyCorrectDefined(GroupType, "groupType", DirectoryPropertySyntax.Enumeration, false, typeof(string), typeof(int));
            CheckPropertyCorrectDefined(DisplayName, "displayName", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(SamAccountName, "samAccountName", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(ObjectClass, "objectClass", ObjectIdentifierString, true, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(DistinguishedName, "distinguishedName", DNString, false, typeof(DN), typeof(string));
            CheckPropertyCorrectDefined(ObjectGuid, "objectGUID", OctetString, false, typeof(Guid), typeof(byte[]));
            CheckPropertyCorrectDefined(Department, "department", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(Description, "description", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(Division, "division", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(Mail, "mail", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(EmployeeId, "employeeId", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(FacsimileTelephoneNumber, "facsimileTelephoneNumber", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(GivenName, "givenName", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(HomeDirectory, "homeDirectory", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(WwwHomePage, "wWWHomePage", UnicodeString, false, typeof(string), typeof(string));
            CheckPropertyCorrectDefined(UserAccountControl, "userAccountControl", DirectoryPropertySyntax.Enumeration, false, typeof(ADS_USER_FLAG), typeof(int));
        }

        [Fact]
        public void OctetStringPropertyShouldCreateCorrectSearchFilterString()
        {
            // Fixture setup
            var guid = Guid.Parse("{3764CBC6-C740-46E3-8291-2C1D7CA3CFA1}");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == OctetString && p.NotionalType == typeof(Guid?));

            // Exercise system
            var results = properties.Select(p => p.CreateSearchFilterString(guid));

            // Verify outcome
            results.ForEach(r => r.Should().Be(@"\c6\cb\64\37\40\c7\e3\46\82\91\2c\1d\7c\a3\cf\a1"));
        }

        [Fact]
        public void GuidPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == OctetString && p.NotionalType == typeof(Guid));

            // Exercise system
            var guids = properties
                .Select(p => p.ConvertToDirectoryValue(guid))
                .OfType<byte[]>()
                .Select(ar => new Guid(ar))
                .ToList();

            // Verify outcome
            guids.Should().HaveCountGreaterThan(0);
            guids.ForEach(r => r.Should().Be(guid));
        }

        [Fact]
        public void GuidPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == OctetString && p.NotionalType == typeof(Guid));

            // Exercise system
            var guids = properties
                .Select(p => p.ConvertFromDirectoryValue(guid.ToByteArray()))
                .ToList();

            // Verify outcome
            guids.Should().HaveCountGreaterThan(0);
            guids.ForEach(r => r.Should().Be(guid));
        }

        [Fact]
        public void DNPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DNString && p.NotionalType == typeof(DN));

            // Exercise system
            var dns = properties
                .Select(p => p.ConvertToDirectoryValue(dn))
                .OfType<string>()
                .Select(DN.Parse)
                .ToList();

            // Verify outcome
            dns.Should().HaveCountGreaterThan(0);
            dns.ForEach(r => r.Should().Be(dn));
        }

        [Fact]
        public void DNPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DNString && p.NotionalType == typeof(DN));

            // Exercise system
            var dns = properties
                .Select(p => p.ConvertFromDirectoryValue("CN=John"))
                .ToList();

            // Verify outcome
            dns.Should().HaveCountGreaterThan(0);
            dns.ForEach(r => r.Should().Be(dn));
        }

        [Fact]
        public void AdsUserFlagPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.Enumeration
                            && p.NotionalType.IsEquivalentTo(typeof(ADS_USER_FLAG)));

            // Exercise system
            var flags = properties
                .Select(p => p.ConvertToDirectoryValue(userFlag))
                .OfType<int>()
                .Select(i => (ADS_USER_FLAG) i)
                .ToList();

            // Verify outcome
            flags.Should().HaveCountGreaterThan(0);
            flags.ForEach(r => r.Should().Be(userFlag));
        }

        [Fact]
        public void AdsUserFlagPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.Enumeration
                            && p.NotionalType.IsEquivalentTo(typeof(ADS_USER_FLAG)));

            // Exercise system
            var flags = properties
                .Select(p => p.ConvertFromDirectoryValue((int) userFlag))
                .ToList();

            // Verify outcome
            flags.Should().HaveCountGreaterThan(0);
            flags.ForEach(f => ((ADS_USER_FLAG) f).Should().Be(userFlag));
        }

        [Fact]
        public void ConvertFromDirectoryValueReturnsNullOnNullInput()
        {
            // Fixture setup

            // Exercise system and verify outcome
            DistinguishedName.ConvertFromDirectoryValue(null).Should().BeNull();
        }

        [Fact]
        public void ExceptionShouldBeThrownForAttemptConvertValueInappropriateSyntax()
        {
            // Fixture setup

            // Exercise system
            Action call = () => DistinguishedName.ConvertFromDirectoryValue("non DN string");

            // Verify outcome
            call.Should().Throw<DirectoryServicesException>()
                .WithMessage("Value 'non DN string' of type 'System.String' not suitable to syntax 'DNString' of directory property 'distinguishedName'.");
        }

        private static void CheckPropertyCorrectDefined(
            DirectoryProperty property,
            string name,
            DirectoryPropertySyntax syntax,
            bool multivalued,
            Type notionalType,
            Type directoryType)
        {
            property.Name.Should().Be(name);
            property.Syntax.Should().Be(syntax);
            property.Multivalued.Should().Be(multivalued);
            property.NotionalType.IsEquivalentTo(notionalType).Should().BeTrue();
            property.DirectoryType.Should().Be(directoryType);
        }
    }
}