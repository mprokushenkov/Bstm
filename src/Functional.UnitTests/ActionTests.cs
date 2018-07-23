using System;
using Xunit;

namespace Bstm.Functional.UnitTests
{
    public class ActionTests
    {
        // ToFunc
        [Fact]
        public void ToFuncShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Action action = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => action.ToFunc());
        }

        [Fact]
        public void ToFuncShouldReturnNotNullFunc()
        {
            // Fixture setup
            Action action = () => { };

            // Exercise system and verify outcome
            Assert.NotNull(action.ToFunc());
        }

        [Fact]
        public void ToFuncShouldInvokeSourceAction()
        {
            // Fixture setup
            var invoked = false;
            Action action = () => invoked = true;

            // Exercise system
            action.ToFunc()();

            // Verify outcome
            Assert.True(invoked);
        }

        // ToFunc`1
        [Fact]
        public void ToFunc1ShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Action<int> action = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => action.ToFunc());
        }

        [Fact]
        public void ToFunc1ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Action<int> action = _ => { };

            // Exercise system and verify outcome
            Assert.NotNull(action.ToFunc());
        }

        [Fact]
        public void ToFunc1ShouldInvokeSourceAction()
        {
            // Fixture setup
            var invoked = false;
            Action<bool> action = b => invoked = b;

            // Exercise system
            action.ToFunc()(true);

            // Verify outcome
            Assert.True(invoked);
        }

        // ToFunc`2
        [Fact]
        public void ToFunc2ShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Action<int, int> action = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => action.ToFunc());
        }

        [Fact]
        public void ToFunc2ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Action<int, int> action = (a, b) => { };

            // Exercise system and verify outcome
            Assert.NotNull(action.ToFunc());
        }

        [Fact]
        public void ToFunc2ShouldInvokeSourceAction()
        {
            // Fixture setup
            var number = 0;
            Action<int, int> action = (a, b) => number = a + b;

            // Exercise system
            action.ToFunc()(1, 2);

            // Verify outcome
            Assert.Equal(3, number);
        }

        // ToFunc`3
        [Fact]
        public void ToFunc3ShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Action<int, int, int> action = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => action.ToFunc());
        }

        [Fact]
        public void ToFunc3ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Action<int, int, int> action = (a, b, c) => { };

            // Exercise system and verify outcome
            Assert.NotNull(action.ToFunc());
        }

        [Fact]
        public void ToFunc3ShouldInvokeSourceAction()
        {
            // Fixture setup
            var number = 0;
            Action<int, int, int> action = (a, b, c) => number = a + b + c;

            // Exercise system
            action.ToFunc()(1, 2, 3);

            // Verify outcome
            Assert.Equal(6, number);
        }

        // ToFunc`3
        [Fact]
        public void ToFunc4ShouldThrowExceptionForNullAction()
        {
            // Fixture setup
            Action<int, int, int, int> action = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => action.ToFunc());
        }

        [Fact]
        public void ToFunc4ShouldReturnNotNullFunc()
        {
            // Fixture setup
            Action<int, int, int, int> action = (a, b, c, d) => { };

            // Exercise system and verify outcome
            Assert.NotNull(action.ToFunc());
        }

        [Fact]
        public void ToFunc4ShouldInvokeSourceAction()
        {
            // Fixture setup
            var number = 0;
            Action<int, int, int, int> action = (a, b, c, d) => number = a + b + c + d;

            // Exercise system
            action.ToFunc()(1, 2, 3, 4);

            // Verify outcome
            Assert.Equal(10, number);
        }
    }
}