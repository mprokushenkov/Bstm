using System;
using Bstm.Common.Primitives;
using FluentAssertions;
using Xunit;

namespace Bstm.Common.UnitTests.Primitives
{
    public sealed class StringLengthConstraintTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("A")]
        [InlineData("ABC")]
        public void ExceptionShouldBeThrownIfValueDoesNotMatchRequiredLength(string value)
        {
            // Fixture setup
            var constraint = new StringLengthConstraint(2);

            // Exercise system
            Action call = () => constraint.CheckConstraint(value);

            // Verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Value length must be equal '2' but '*'.");

        }
    }
}