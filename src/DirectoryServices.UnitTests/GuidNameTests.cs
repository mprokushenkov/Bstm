using System;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class GuidNameTests : TestBase
    {
        private readonly GuidName guidName = new GuidName(Guid.Parse("da1432ec-c560-40cf-9a75-bc8b77336082"));

        public GuidNameTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void ToStringReturnsLdapGuidName()
        {
            // Fixture setup

            // Exercise system and verify outcome
            guidName.ToString().Should().Be("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionShouldBeThrownForNullOrWhiteSpaceValueOnParse(string value)
        {
            // Fixture setup

            // Exercise system
            Action call = () => GuidName.Parse(value);

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("GuidName string can not be null or white space.");
        }

        [Fact]
        public void CorrectExceptionShouldBeThrownForInvalidValueOnParse()
        {
            // Fixture setup

            // Exercise system
            Action call = () => GuidName.Parse("Hello");

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("String 'Hello' can not be converted to valid GuidName instance.");
        }

        [Fact]
        public void ParseReturnsGuidName()
        {
            // Fixture setup

            // Exercise system
            var parseResult = GuidName.Parse("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");

            // Verify outcome
            parseResult.ToString().Should().Be("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");
        }

        [Fact]
        public void TryParseReturnsFalseOnInvalidValue()
        {
            // Fixture setup

            // Exercise system
            var success = GuidName.TryParse("Hello", out var result);

            // Verify outcome
            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsTrueOnCorrectValue()
        {
            // Fixture setup

            // Exercise system
            var success = GuidName.TryParse("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>", out var result);

            // Verify outcome
            success.Should().BeTrue();
            result.ToString().Should().Be("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            EqualityTestsFor<GuidName>.Assert();
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsCorrectGuidNameRepresentation()
        {
            // Fixture setup
            string actual = guidName;

            // Exercise system and verify outcome
            actual.Should().Be("<GUID=da1432ec-c560-40cf-9a75-bc8b77336082>");
        }
    }
}