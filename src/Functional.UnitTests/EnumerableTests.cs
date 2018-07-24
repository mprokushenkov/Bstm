using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bstm.Functional.UnitTests
{
    public sealed class EnumerableTests
    {
        [Fact]
        public void MapShouldThrowExceptionForNullEnumerable()
        {
            // Fixture setup
            IEnumerable<int> enumerable = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => enumerable.Map(i => i.ToString()));
        }

        [Fact]
        public void MapShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            var enumerable = Enumerable.Empty<int>();

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => enumerable.Map<int, string>(null));
        }

        [Fact]
        public void MapShouldInvokeConverter()
        {
            // Fixture setup
            var range = Enumerable.Range(0, 3);

            // Exercise system
            var result = range.Map(i => i.ToString()).ToArray();

            // Verify outcome
            Assert.Equal(new[] { "0", "1", "2" }, result);
        }

        [Fact]
        public void BindShouldThrowExceptionForNullEnumerable()
        {
            // Fixture setup
            IEnumerable<int> enumerable = null;

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => enumerable.Bind<int, string>(_ => Enumerable.Empty<string>()));
        }

        [Fact]
        public void BindShouldThrowExceptionForNullFunc()
        {
            // Fixture setup
            var enumerable = Enumerable.Empty<int>();

            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => enumerable.Bind<int, string>(null));
        }

        [Fact]
        public void BindShouldInvokeConverter()
        {
            // Fixture setup
            var items = new[]
            {
                new { Numbers = Enumerable.Range(0, 2) },
                new { Numbers = Enumerable.Range(2, 2) },
                new { Numbers = Enumerable.Range(4, 2) }
            };

            // Exercise system
            var result = items.Bind(i => i.Numbers).ToArray();

            // Verify outcome
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, result);
        }
    }
}