using System;
using static Bstm.Common.Guard;

namespace Bstm.Functional
{
    public static class FuncExtensions
    {
        public static Func<TResult> Apply<T, TResult>(this Func<T, TResult> func, T t)
        {
            CheckNull(func, nameof(func));
            return () => func(t);
        }

        public static Func<T2, TResult> Apply<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 t1)
        {
            CheckNull(func, nameof(func));
            return t2 => func(t1, t2);
        }

        public static Func<T2, T3, TResult> Apply<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 t1)
        {
            CheckNull(func, nameof(func));
            return (t2, t3) => func(t1, t2, t3);
        }

        public static Func<T2, T3, T4, TResult> Apply<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> func, T1 t1)
        {
            CheckNull(func, nameof(func));
            return (t2, t3, t4) => func(t1, t2, t3, t4);
        }

        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            CheckNull(func, nameof(func));
            return t1 => t2 => func(t1, t2);
        }

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, TResult> func)
        {
            CheckNull(func, nameof(func));
            return t1 => t2 => t3 => func(t1, t2, t3);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> func)
        {
            CheckNull(func, nameof(func));
            return t1 => t2 => t3 => t4 => func(t1, t2, t3, t4);
        }
    }
}