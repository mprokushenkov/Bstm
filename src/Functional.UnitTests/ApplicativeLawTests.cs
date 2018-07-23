using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Bstm.Functional.UnitTests
{
    public class ApplicativeLawTests
    {
        private readonly Func<int, int, int> multiply = (x, y) => x * y;

        [Property(Arbitrary = new[] { typeof(ApplicativeLawTests) })]
        public void ApplicativeLawHolds(Maybe<int> a, Maybe<int> b)
        {
            var first = multiply.ToMaybe().Apply(a).Apply(b);
            var second = a.Map(multiply).Apply(b);

            Assert.Equal(first, second);
        }

        public static Arbitrary<Maybe<T>> GenerateMaybe<T>()
        {
            var result = from hasValue in Arb.Generate<bool>()
                         from val in Arb.Generate<T>()
                         select hasValue && val != null ? val.ToMaybe() : Maybe<T>.Nothing;

            return result.ToArbitrary();
        }
    }
}