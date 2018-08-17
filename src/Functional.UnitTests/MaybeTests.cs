using System;
using Xunit;

namespace Bstm.Functional.UnitTests
{
    public sealed class MaybeTests
    {
        [Fact]
        public void CtorShouldThrowExceptionForNullValue()
        {
            // Fixture setup

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new Maybe<string>(null));
        }

        [Fact]
        public void NothingShouldReturnsDefaultMaybe()
        {
            // Fixture setup

            // Exercise system and verify outcome
            Assert.Equal(default(Maybe<string>), Maybe<string>.Nothing);
        }

        [Fact]
        public void ImplicitNullValueCastShouldReturnsNothing()
        {
            // Fixture setup
            const string value = null;

            // Exercise system and verify outcome
            Assert.Equal(Maybe<string>.Nothing, value);
        }

        [Fact]
        public void ImplicitNotNullValueCastShouldReturnsJust()
        {
            // Fixture setup
            const string value = "Hello";
            var expected = new Maybe<string>(value);

            // Exercise system and verify outcome
            Assert.Equal(expected, value);
        }

        [Fact]
        public void MatchShouldReturnsGetNothingResultForNullValue()
        {
            // Fixture setup
            Maybe<string> maybe = null;

            // Exercise system
            var result = maybe.Match(() => string.Empty, v => v);

            // Verify outcome
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MatchShouldReturnsGetJustResultForNotNullValue()
        {
            // Fixture setup
            const string value = "hello";
            Maybe<string> maybe = value;

            // Exercise system
            var result = maybe.Match(() => string.Empty, v => v);

            // Verify outcome
            Assert.Equal(value, result);
        }

        [Fact]
        public void BindShouldReturnsNothingForNullValue()
        {
            // Fixture setup
            Maybe<int?> maybe = null;
            Func<int?, Maybe<string>> getMaybe = i => true.ToString().ToMaybe();

            // Exercise system
            var result = maybe.Bind(getMaybe);

            // Verify outcome
            Assert.Equal(Maybe<string>.Nothing, result);
        }

        [Fact]
        public void BindShouldReturnsJustForNotNullValue()
        {
            // Fixture setup
            Maybe<int?> maybe = 25;
            Func<int?, Maybe<string>> getMaybe = i => i.ToString().ToMaybe();

            // Exercise system
            var result = maybe.Bind(getMaybe);

            // Verify outcome
            Assert.Equal("25".ToMaybe(), result);
        }

        [Fact]
        public void EqualityOperatorShouldReturnTrueForEqualArguments()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 25;

            // Exercise system and verify outcome
            Assert.True(left == right);
        }

        [Fact]
        public void EqualityOperatorShouldReturnFalseForNonEqualArguments()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 26;

            // Exercise system and verify outcome
            Assert.False(left == right);
        }

        [Fact]
        public void InequalityOperatorShouldReturnTrueForNonEqualArguments()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 26;

            // Exercise system and verify outcome
            Assert.True(left != right);
        }

        [Fact]
        public void InqualityOperatorShouldReturnFalseForEqualArguments()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 25;

            // Exercise system and verify outcome
            Assert.False(left != right);
        }

        [Fact]
        public void MaybeEqualsShouldReturnTrueForEqualInstances()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 25;

            // Exercise system and verify outcome
            Assert.True(left.Equals(right));
        }

        [Fact]
        public void MaybeEqualsShouldReturnFalseForNonEqualInstances()
        {
            // Fixture setup
            Maybe<int> left = 25;
            Maybe<int> right = 26;

            // Exercise system and verify outcome
            Assert.False(left.Equals(right));
        }

        [Fact]
        public void ObjectEqualsShouldReturnFalseForNull()
        {
            // Fixture setup
            Maybe<int> left = 25;

            // Exercise system and verify outcome
            Assert.False(left.Equals(null));
        }

        [Fact]
        public void ObjectEqualsShouldReturnFalseForNoMaybe()
        {
            // Fixture setup
            Maybe<int> left = 25;

            // Exercise system and verify outcome
            Assert.False(left.Equals("null"));
        }

        [Fact]
        public void ObjectEqualsShouldReturnTrueForEqualInstances()
        {
            // Fixture setup
            Maybe<int> left = 25;
            object right = 25.ToMaybe();

            // Exercise system and verify outcome
            Assert.True(left.Equals(right));
        }

        [Fact]
        public void ObjectEqualsShouldReturnFalseForNonEqualInstances()
        {
            // Fixture setup
            Maybe<int> left = 25;
            object right = 26.ToMaybe();

            // Exercise system and verify outcome
            Assert.False(left.Equals(right));
        }

        [Fact]
        public void GetHashCodeShouldReturnValueHashCode()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system and verify outcome
            Assert.Equal(25.GetHashCode(), maybe.GetHashCode());
        }

        [Fact]
        public void GetHashCodeShouldReturnZeroIfNoValue()
        {
            // Fixture setup
            Maybe<int> maybe = new Maybe<int>();

            // Exercise system and verify outcome
            Assert.Equal(0, maybe.GetHashCode());
        }

        [Fact]
        public void ToStringShouldReturnValueString()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system and verify outcome
            Assert.Equal("25", maybe.ToString());
        }

        [Fact]
        public void ToStringShouldReturnNothingIfNoValue()
        {
            // Fixture setup
            Maybe<int> maybe = new Maybe<int>();

            // Exercise system and verify outcome
            Assert.Equal("Nothing", maybe.ToString());
        }

        [Fact]
        public void MapShouldThrowExceptionForNullConverter()
        {
            // Fixture setup
            Func<int, string> converter = null;
            Maybe<int> maybe = 25;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => maybe.Map(converter));
        }

        [Fact]
        public void MapShouldNotInvokeConverterIfNoValue()
        {
            // Fixture setup
            var maybe = new Maybe<int>();

            // Exercise system
            var result = maybe.Map(i => i.ToString());

            // Verifiy outcome
            Assert.Equal(Maybe<string>.Nothing, result);
        }

        [Fact]
        public void MapShouldInvokeConverter()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system
            var result = maybe.Map(i => i.ToString());

            // Verifiy outcome
            Assert.Equal("25".ToMaybe(), result);
        }

        [Fact]
        public void DoShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => maybe.Do(null));
        }

        [Fact]
        public void DoShouldNotMakeSideEffectIfNoValue()
        {
            // Fixture setup
            var invoked = false;
            var maybe = new Maybe<int>();

            // Exercise system
            var result = maybe.Do(_ => invoked = true);

            // Verifiy outcome
            Assert.False(invoked);
        }

        [Fact]
        public void DoShouldMakeSideEffect()
        {
            // Fixture setup
            var invoked = false;
            Maybe<int> maybe = 25;

            // Exercise system
            var result = maybe.Do(_ => invoked = true);

            // Verifiy outcome
            Assert.True(invoked);
        }

        [Fact]
        public void WhereShouldReturnNothingIfNoValue()
        {
            // Fixture setup
            var maybe = new Maybe<int>();

            // Exercise system
            var result = maybe.Where(i => i == 0);

            // Verifiy outcome
            Assert.Equal(Maybe<int>.Nothing, result);
        }

        [Fact]
        public void WhereShouldReturnNothingIfPredicateFails()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system
            var result = maybe.Where(i => i == 0);

            // Verifiy outcome
            Assert.Equal(Maybe<int>.Nothing, result);
        }

        [Fact]
        public void WhereShouldReturnMaybeForSuccessPredicate()
        {
            // Fixture setup
            Maybe<int> maybe = 25;

            // Exercise system
            var result = maybe.Where(i => i == 25);

            // Verifiy outcome
            Assert.Equal(maybe, result);
        }

        [Fact]
        public void Apply2ShouldReturnNothingForEmptyArgument()
        {
            // Fixture setup
            Func<int, string> simpleFunc = a => a.ToString();
            var funcMaybe = simpleFunc.ToMaybe();

            // Exercise system and verify outcome
            var result = funcMaybe.Apply(default(Maybe<int>));

            // Verify outcome
            Assert.Equal(Maybe<string>.Nothing, result);
        }

        [Fact]
        public void Apply2ShouldReturnNothingForEmptyFunc()
        {
            // Fixture setup
            var funcMaybe = default(Maybe<Func<int, string>>);

            // Exercise system and verify outcome
            var result = funcMaybe.Apply(default(Maybe<int>));

            // Verify outcome
            Assert.Equal(Maybe<string>.Nothing, result);
        }

        [Fact]
        public void Apply2ShouldReturnCorrectMaybe()
        {
            // Fixture setup
            Func<int, int> simpleFunc = a => a + a;
            var funcMaybe = simpleFunc.ToMaybe();

            // Exercise system and verify outcome
            var result = funcMaybe.Apply(2);

            // Verify outcome
            Assert.Equal(4, result);
        }
    }
}