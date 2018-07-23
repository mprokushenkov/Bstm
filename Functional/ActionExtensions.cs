using System;
using Bstm.Common;

namespace Bstm.Functional
{
    public static class ActionExtensions
    {
        public static Func<Unit> ToFunc(this Action action)
        {
            Guard.CheckNull(action, nameof(action));
            return () => { action(); return Unit.Default; };
        }

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
        {
            Guard.CheckNull(action, nameof(action));
            return t => { action(t); return Unit.Default; };
        }

        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
        {
            Guard.CheckNull(action, nameof(action));
            return (t1 , t2) => { action(t1, t2); return Unit.Default; };
        }

        public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> action)
        {
            Guard.CheckNull(action, nameof(action));
            return (t1 , t2, t3) => { action(t1, t2, t3); return Unit.Default; };
        }

        public static Func<T1, T2, T3, T4, Unit> ToFunc<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action)
        {
            Guard.CheckNull(action, nameof(action));
            return (t1 , t2, t3, t4) => { action(t1, t2, t3, t4); return Unit.Default; };
        }
    }
}