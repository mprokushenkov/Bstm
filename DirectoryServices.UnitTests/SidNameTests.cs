using System;
using System.Security.Principal;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class SidNameTests : TestBase
    {
        private readonly SidName sidName = new SidName(new SecurityIdentifier("S-1-5-32-544"));

        public SidNameTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void CtorThrowsExceptionForNullArguments()
        {
            // Fixture setup
            Fixture.Inject(new SecurityIdentifier("S-1-5-32-544"));
            var assertion = new GuardClauseAssertion(Fixture);

            // Exercise system and verify outcome
            assertion.Verify(typeof(SidName).GetConstructors());
        }

        [Fact]
        public void ToStringReturnsLdapSidName()
        {
            // Fixture setup

            // Exercise system and verify outcome
            sidName.ToString().Should().Be("<SID=S-1-5-32-544>");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionShouldBeThrownForNullOrWhiteSpaceValueOnParse(string value)
        {
            // Fixture setup

            // Exercise system
            Action call = () => SidName.Parse(value);

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("SidName string can not be null or white space.");
        }

        [Fact]
        public void CorrectExceptionShouldBeThrownForInvalidValueOnParse()
        {
            // Fixture setup

            // Exercise system
            Action call = () => SidName.Parse("Hello");

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("String 'Hello' can not be converted to valid SidName instance.");
        }

        [Fact]
        public void ParseReturnsSidName()
        {
            // Fixture setup

            // Exercise system
            var parseResult = SidName.Parse("<SID=S-1-5-32-544>");

            // Verify outcome
            parseResult.ToString().Should().Be("<SID=S-1-5-32-544>");
        }

        [Fact]
        public void TryParseReturnsFalseOnInvalidValue()
        {
            // Fixture setup

            // Exercise system
            var success = SidName.TryParse("Hello", out var result);

            // Verify outcome
            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsTrueOnCorrectValue()
        {
            // Fixture setup

            // Exercise system
            var success = SidName.TryParse("<SID=S-1-5-32-544>", out var result);

            // Verify outcome
            success.Should().BeTrue();
            result.ToString().Should().Be("<SID=S-1-5-32-544>");
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup
            Fixture.Inject(new SecurityIdentifier("S-1-5-32-544"));
            var otherName = new SidName(new SecurityIdentifier("S-1-5-32-545"));

            var configuration = EqualityTestsConfigurer<SidName>
                .Instance(sidName)
                .ShouldBeEqualTo(sidName)
                .ShouldNotBeEqualTo(otherName);

            // Exercise system and verify outcome
            EqualityTestsFor<SidName>.Assert(() => configuration, Fixture);
        }

        [Fact]
        public void ImplicitReturnsConversionOperatorCorrectRdnRepresentation()
        {
            // Fixture setup
            string actual = sidName;

            // Exercise system and verify outcome
            actual.Should().Be("<SID=S-1-5-32-544>");
        }
    }
}