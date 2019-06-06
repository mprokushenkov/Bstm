using JetBrains.Annotations;

namespace Bstm.Common.Primitives
{
    public abstract class ConstrainedString
    {
        protected ConstrainedString([NotNull] string value, params IStringConstraint[] constraints)
        {
            Guard.CheckNull(value, nameof(value));
            constraints.ForEach(c => c.CheckConstraint(value));
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;

        public static implicit operator string(ConstrainedString cs) => cs?.Value;
    }
}