using System;

namespace Bstm.Common.Primitives
{
    public sealed class StringLengthConstraint : IStringConstraint
    {
        private readonly int length;

        public StringLengthConstraint(int length) => this.length = length;

        public void CheckConstraint(string value)
        {
            if (value?.Length != length)
            {
                var message = string.Format(
                    ErrorMessages.ArgumentOutOfRangeException_StringLengthConstraint,
                    length,
                    value?.Length);

                throw new ArgumentOutOfRangeException(null, message);
            }
        }
    }
}