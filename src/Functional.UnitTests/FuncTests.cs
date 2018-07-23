using System;
using Xunit;

namespace Bstm.Functional.UnitTests
{
    public class FuncTests
    {
        // Curry`3
        [Fact]
        public void Curry3ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Curry());
        }

        [Fact]
        public void Curry3ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int> func = (x, y) => x + y;

            // Exercise system and verify outcome
            Assert.NotNull(func.Curry());
        }

        [Fact]
        public void Curry3ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int> func = (a, b) => a + b;

            // Exercise system and verify outcome
            var result = func.Curry()(1)(2);

            // Verify outcome
            Assert.Equal(3, result);
        }

        // Curry`4
        [Fact]
        public void Curry4ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Curry());
        }

        [Fact]
        public void Curry4ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = (a, b, c) => a + b + c;

            // Exercise system and verify outcome
            Assert.NotNull(func.Curry());
        }

        [Fact]
        public void Curry4ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = (a, b, c) => a + b + c;

            // Exercise system and verify outcome
            var result = func.Curry()(1)(2)(3);

            // Verify outcome
            Assert.Equal(6, result);
        }

        // Curry`5
        [Fact]
        public void Curry5ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Curry());
        }

        [Fact]
        public void Curry5ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;

            // Exercise system and verify outcome
            Assert.NotNull(func.Curry());
        }

        [Fact]
        public void Curry5ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;

            // Exercise system and verify outcome
            var result = func.Curry()(1)(2)(3)(4);

            // Verify outcome
            Assert.Equal(10, result);
        }

        // Apply`2
        [Fact]
        public void Apply2ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Apply(1));
        }

        [Fact]
        public void Apply2ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int> func = (a) => a * 2;

            // Exercise system and verify outcome
            Assert.NotNull(func.Apply(1));
        }

        [Fact]
        public void Apply2ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int> func = (a) => a * 2;

            // Exercise system and verify outcome
            var result = func.Apply(1)();

            // Verify outcome
            Assert.Equal(2, result);
        }

        // Apply`3
        [Fact]
        public void Apply3ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Apply(1));
        }

        [Fact]
        public void Apply3ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int> func = (a, b) => a + b;

            // Exercise system and verify outcome
            Assert.NotNull(func.Apply(1));
        }

        [Fact]
        public void Apply3ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int> func = (a, b) => a + b;

            // Exercise system and verify outcome
            var result = func.Apply(1).Apply(2)();

            // Verify outcome
            Assert.Equal(3, result);
        }

        // Apply`4
        [Fact]
        public void Apply4ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Apply(1));
        }

        [Fact]
        public void Apply4ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = (a, b, c) => a + b + c;

            // Exercise system and verify outcome
            Assert.NotNull(func.Apply(1));
        }

        [Fact]
        public void Apply4ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int, int> func = (a, b, c) => a + b + c;

            // Exercise system and verify outcome
            var result = func.Apply(1).Apply(2).Apply(3)();

            // Verify outcome
            Assert.Equal(6, result);
        }

        // Apply`5
        [Fact]
        public void Apply5ShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => func.Apply(1));
        }

        [Fact]
        public void Apply5ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;

            // Exercise system and verify outcome
            Assert.NotNull(func.Apply(1));
        }

        [Fact]
        public void Apply5ShouldReturnCorrectFunc()
        {
            // Fixture setup
            Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;

            // Exercise system and verify outcome
            var result = func.Apply(1).Apply(2).Apply(3).Apply(4)();

            // Verify outcome
            Assert.Equal(10, result);
        }
    }
}