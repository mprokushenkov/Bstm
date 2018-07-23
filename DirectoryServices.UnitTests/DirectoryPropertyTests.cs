using System;
using System.Linq;
using AutoFixture.Idioms;
using Bstm.Common;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

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
            CheckPropertyCorrectDefined(DirectoryProperty.Member, "member", DirectoryPropertySyntax.DNString, true);
            CheckPropertyCorrectDefined(DirectoryProperty.GroupType, "groupType", DirectoryPropertySyntax.Enumeration, false);
            CheckPropertyCorrectDefined(DirectoryProperty.DisplayName, "displayName", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.SamAccountName, "samAccountName", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.ObjectClass, "objectClass", DirectoryPropertySyntax.ObjectIdentifierString, true);
            CheckPropertyCorrectDefined(DirectoryProperty.DistinguishedName, "distinguishedName", DirectoryPropertySyntax.DNString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.ObjectGuid, "objectGUID", DirectoryPropertySyntax.OctetString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.Department, "department", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.Description, "description", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.Division, "division", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.Mail, "mail", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.EmployeeId, "employeeId", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.FacsimileTelephoneNumber, "facsimileTelephoneNumber", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.GivenName, "givenName", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.HomeDirectory, "homeDirectory", DirectoryPropertySyntax.UnicodeString, false);
            CheckPropertyCorrectDefined(DirectoryProperty.WwwHomePage, "wWWHomePage", DirectoryPropertySyntax.UnicodeString, false);
        }

        [Fact]
        public void OctetStringPropertyShouldCreateCorrectSearchFilterString()
        {
            // Fixture setup
            var guid = Guid.Parse("{3764CBC6-C740-46E3-8291-2C1D7CA3CFA1}");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid?));

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
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid));

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
        public void DNPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.DNString && p.NotionalType == typeof(DN));

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
        public void ConvertFromDirectoryValueReturnsNullOnNullInput()
        {
            // Fixture setup

            // Exercise system and verify outcome
            DirectoryProperty.DistinguishedName.ConvertFromDirectoryValue(null).Should().BeNull();
        }

        [Fact]
        public void GuidPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid));

            // Exercise system
            var guids = properties
                .Select(p => p.ConvertFromDirectoryValue(guid.ToByteArray()))
                .ToList();

            // Verify outcome
            guids.Should().HaveCountGreaterThan(0);
            guids.ForEach(r => r.Should().Be(guid));
        }

        [Fact]
        public void DNPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Common.Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.DNString && p.NotionalType == typeof(DN));

            // Exercise system
            var dns = properties
                .Select(p => p.ConvertFromDirectoryValue("CN=John"))
                .ToList();

            // Verify outcome
            dns.Should().HaveCountGreaterThan(0);
            dns.ForEach(r => r.Should().Be(dn));
        }

        [Fact]
        public void ExceptionShouldBeThrownForAttemptConvertValueInappropriateSyntax()
        {
            // Fixture setup

            // Exercise system
            Action call = () => DirectoryProperty.DistinguishedName.ConvertFromDirectoryValue("non DN string");

            // Verify outcome
            call.Should().Throw<DirectoryServicesException>()
                .WithMessage("Value 'non DN string' of type 'System.String' not suitable to syntax 'DNString' of directory property 'distinguishedName'.");
        }

        private static void CheckPropertyCorrectDefined(
            DirectoryProperty property,
            string name,
            DirectoryPropertySyntax syntax,
            bool multivalued)
        {
            property.Name.Should().Be(name);
            property.Syntax.Should().Be(syntax);
            property.Multivalued.Should().Be(multivalued);
        }
    }
}