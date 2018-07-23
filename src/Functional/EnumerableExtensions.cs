using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Bstm.Common.Guard;

namespace Bstm.Functional
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> converter)
        {
            CheckNull(enumerable, nameof(enumerable));
            CheckNull(converter, nameof(converter));

            return enumerable.Select(converter);
        }

        public static IEnumerable<TResult> Bind<T, TResult>(
            this IEnumerable<T> enumerable,
            Func<T, IEnumerable<TResult>> converter)
        {
            CheckNull(enumerable, nameof(enumerable));
            CheckNull(converter, nameof(converter));

            return enumerable.SelectMany(converter);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            CheckNull(enumerable, nameof(enumerable));
            CheckNull(action, nameof(action));

            enumerable.Map(action.ToFunc()).ToImmutableArray();
        }
    }
}