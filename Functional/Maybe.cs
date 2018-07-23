using System;
using System.Collections.Generic;
using static Bstm.Common.Guard;

namespace Bstm.Functional
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T value;
        private readonly bool hasValue;

        public Maybe(T value) : this()
        {
            this.value = CheckNull(value, nameof(value));
            hasValue = true;
        }

        public static readonly Maybe<T> Nothing = default(Maybe<T>);

        public static implicit operator Maybe<T>(T value) => value != null ? new Maybe<T>(value) : Nothing;

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        public TResult Match<TResult>(Func<TResult> getNothing, Func<T, TResult> getJust)
            => hasValue ? getJust(value) : getNothing();

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> getMaybe)
            => hasValue ? getMaybe(value) : Maybe<TResult>.Nothing;

        public bool Equals(Maybe<T> other) => EqualityComparer<T>.Default.Equals(value, other.value);

        public override bool Equals(object obj) => obj is Maybe<T> maybe && Equals(maybe);

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(value);

        public override string ToString() => hasValue ? value.ToString() : nameof(Nothing);
    }
}