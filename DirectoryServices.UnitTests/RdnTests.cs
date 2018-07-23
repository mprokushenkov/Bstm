using System;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class RdnTests : TestBase
    {
        public RdnTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void CtorThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(Rdn).GetConstructors());
        }

        [Fact]
        public void ToStringReturnsCorrectRdnRepresentation()
        {
            // Fixture setup
            var rdn = new Rdn(NamingAttribute.Cn, new LdapName("John"));

            // Exercise system and verify outcome
            rdn.ToString().Should().Be("CN=John");
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsCorrectRdnRepresentation()
        {
            // Fixture setup
            string rdn = new Rdn(NamingAttribute.Cn, new LdapName("John"));

            // Exercise system and verify outcome
            rdn.Should().Be("CN=John");
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup
            var configuration = EqualityTestsConfigurer<Rdn>
                .Instance(Rdn.Parse("CN=John"))
                .ShouldBeEqualTo(Rdn.Parse("CN=John"))
                .ShouldNotBeEqualTo(Rdn.Parse("CN=George"));

            // Exercise system and verify outcome
            EqualityTestsFor<Rdn>.Assert(() => configuration);
        }

        [Fact]
        public void ExceptionThrownForCandidateWithoutEqualitySign()
        {
            // Fixture setup
            Action call = () => Rdn.Parse("blah-blah-blah");

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("String 'blah-blah-blah' can not be converted to valid Rdn instance.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CorrectExceptionThrownForInvalidValueOnParse(string value)
        {
            // Fixture setup
            Action call = () => Rdn.Parse(value);

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Rdn string can not be null or white space.");
        }

        [Fact]
        public void TryParseReturnsFalseOnInvalidValue()
        {
            // Fixture setup

            // Exercise system
            var success = Rdn.TryParse("Hello", out var result);

            // Verify outcome
            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [Fact]
        public void TryParseReturnsTrueOnCorrectValue()
        {
            // Fixture setup

            // Exercise system
            var success = Rdn.TryParse("CN=John", out var result);

            // Verify outcome
            success.Should().BeTrue();
            result.ToString().Should().Be("CN=John");
        }
    }
}