using System;
using AutoFixture;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class LdapNameTests : TestBase
    {
        public LdapNameTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionThrownForInvalidValue(string value)
        {
            // Fixture setup
            // ReSharper disable once ObjectCreationAsStatement
            Action call = () => new LdapName(value);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("LdapName string can not be null or white space.");
        }

        [Fact]
        public void ToStringReturnsValueString()
        {
            // Fixture setup
            var value = Fixture.Create<string>();

            // Exercise system
            var name = new LdapName(value);

            // Verify outcome
            name.ToString().Should().Be(value);
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsValueString()
        {
            // Fixture setup
            var value = Fixture.Create<string>();

            // Exercise system
            string name = new LdapName(value);

            // Verify outcome
            name.Should().Be(value);
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsLdapName()
        {
            // Fixture setup
            var ldapName = Fixture.Create<LdapName>();

            // Exercise system
            string stringName = ldapName;

            // Verify outcome
            stringName.Should().Be(ldapName.ToString());
        }

        [Fact]
        public void SpecialCharactersEscaped()
        {
            // Fixture setup

            const string value = @",#+< \ >;""=";
            const string expected = @"\,\#\+\< \\ \>\;\""\=";

            // Exercise system
            var name = new LdapName(value);

            // Verify outcome
            name.ToString().Should().Be(expected);
        }

        [Fact]
        public void LeadingAndTrailingSpacesEscaped()
        {
            // Fixture setup
            const string value = " Hello, World ";
            const string expected = "\\ Hello\\, World\\ ";

            // Exercise system
            var name = new LdapName(value);

            // Verify outcome
            name.ToString().Should().Be(expected);
        }

        [Fact]
        public void NoEscapeIfSymbolAlreadyEscaped()
        {
            // Fixture setup
            const string value = @"John\, Doe";

            // Exercise system
            var name = new LdapName(value);

            // Verify outcome
            name.ToString().Should().Be(value);
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            EqualityTestsFor<LdapName>.Assert();

            // Verify outcome
        }

        [Theory]
        [AutoMoqData]
        public void GetHashCodeReturnsValueHashCode(string value)
        {
            // Fixture setup
            var name = new LdapName(value);

            // Exercise system and verify outcome
            (value.GetHashCode() == name.GetHashCode()).Should().BeTrue();
        }
    }
}