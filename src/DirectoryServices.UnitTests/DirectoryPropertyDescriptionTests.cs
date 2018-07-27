using System;
using System.Linq;
using ActiveDs;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Bstm.DirectoryServices.DirectoryProperty;
using static Bstm.DirectoryServices.DirectoryPropertySyntax;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DirectoryPropertyDescriptionTests : TestBase
    {
        public DirectoryPropertyDescriptionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
            CheckPropertyCorrectDefined(GroupType, "groupType", Enumeration, false, typeof(string), typeof(int));
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
            CheckPropertyCorrectDefined(UserAccountControl, "userAccountControl", Enumeration, false, typeof(ADS_USER_FLAG), typeof(int));
            CheckPropertyCorrectDefined(AccountExpires, "accountExpires", Interval, false, typeof(DateTimeOffset?), typeof(IADsLargeInteger));
            CheckPropertyCorrectDefined(BadPwdCount, "badPwdCount", Enumeration, false, typeof(int), typeof(int));
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
            property.DirectoryType.IsEquivalentTo(directoryType).Should().BeTrue();
        }
    }
}