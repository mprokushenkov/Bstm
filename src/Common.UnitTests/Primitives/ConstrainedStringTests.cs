using System;
using Bstm.Common.Primitives;
using FluentAssertions;
using Xunit;

namespace Bstm.Common.UnitTests.Primitives
{
    public sealed class ConstrainedStringTests
    {
        [Fact]
        public void ToStringShouldReturnValue()
        {
            // Fixture setup
            var string2 = new String2("AB");

            // Exercise system and verify outcome
            string2.ToString().Should().Be(string2.Value);
        }

        [Fact]
        public void ExceptionShouldBeThrownForNullValue()
        {
            // Fixture setup

            // Exercise system
            Action call = () => new String2(null);

            // Verify outcome
            call.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("A")]
        [InlineData("ABC")]
        public void ExceptionShouldBeThrownIfValueDoesNotMatchRequiredLength(string value)
        {
            // Fixture setup

            // Exercise system
            Action call = () => new String2(value);

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Value length must be equal '2' but '*'.");
        }

        [Fact]
        public void ImplicitCastToStringShouldBeCorrect()
        {
            // Fixture setup

            // Exercise system
            string actual = new String2("AB");

            // Verify outcome
            actual.Should().Be("AB");
        }

        [Fact]
        public void ImplicitCastToStringShouldBeSuccessIfSourceIsNull()
        {
            // Fixture setup

            // Exercise system
            // ReSharper disable once ExpressionIsAlwaysNull
            string actual = default(String2);

            // Verify outcome
            actual.Should().BeNull();
        }

        private class String2 : ConstrainedString
        {
            public String2(string value) : base(value, new StringLengthConstraint(2))
            {
            }
        }
    }
}