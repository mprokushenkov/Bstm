using System;
using static Bstm.Common.Guard;

namespace Bstm.Functional
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value) => value;

        public static Maybe<TResult> Map<T, TResult>(this Maybe<T> maybe, Func<T, TResult> convert)
        {
            CheckNull(convert, nameof(convert));

            return maybe.Match(
                () => Maybe<TResult>.Nothing,
                t => convert(t));
        }

        public static Maybe<Func<T2, TResult>> Map<T1, T2, TResult>(this Maybe<T1> maybe, Func<T1, T2, TResult> func)
            => maybe.Map(func.Curry());

        public static Maybe<Func<T2, Func<T3, TResult>>> Map<T1, T2, T3, TResult>(
            this Maybe<T1> maybe, Func<T1, T2, T3, TResult> func)
                => maybe.Map(func.Curry());

        public static Maybe<Func<T2, Func<T3, Func<T4, TResult>>>> Map<T1, T2, T3, T4, TResult>(
            this Maybe<T1> maybe, Func<T1, T2, T3, T4, TResult> func)
                => maybe.Map(func.Curry());

        public static Maybe<Unit> Do<T>(this Maybe<T> maybe, Action<T> action)
            => maybe.Map(action.ToFunc());

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
            => maybe.Match(
                () => Maybe<T>.Nothing,
                t => predicate(t) ? maybe : Maybe<T>.Nothing) ;

        public static Maybe<TResult> Apply<T, TResult>(this Maybe<Func<T, TResult>> maybeF, Maybe<T> maybeT)
            => maybeF.Match(
                () => Maybe<TResult>.Nothing,
                f => maybeT.Match(
                    () => Maybe<TResult>.Nothing,
                    t => f(t)
                )
            );

        public static Maybe<Func<T2, TResult>> Apply<T1, T2, TResult>(
            this Maybe<Func<T1, T2, TResult>> maybeF, Maybe<T1> maybeT)
                => Apply(maybeF.Map(FuncExtensions.Curry), maybeT);
    }
}