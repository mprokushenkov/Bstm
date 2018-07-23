using System;
using System.Security.Principal;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class SidName : IAdsObjectName, IEquatable<SidName>
    {
        private readonly SecurityIdentifier value;
        private string cachedStringValue;

        public SidName([NotNull] SecurityIdentifier value)
        {
            this.value = Guard.CheckNull(value, nameof(value));
        }

        [NotNull]
        public static SidName Parse([NotNull] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    string.Format(ErrorMessages.SidName_ArgumentOutOfRangeException_InvalidValue));
            }

            try
            {
                var candidate = value.Trim('<', '>').Replace("SID=", string.Empty);
                return new SidName(new SecurityIdentifier(candidate));
            }
            catch (ArgumentException)
            {
                var message = string.Format(
                    ErrorMessages.SidName_ArgumentOutOfRangeException_InvalidParseValue,
                    value);

                throw new ArgumentOutOfRangeException(null, message);
            }
        }

        public static bool TryParse([NotNull] string value, out SidName sidName)
        {
            try
            {
                sidName = Parse(value);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                sidName = null;
                return false;
            }
        }

        public static implicit operator string(SidName name)
        {
            return name?.ToString();
        }

        public static bool operator ==(SidName left, SidName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SidName left, SidName right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return cachedStringValue ?? (cachedStringValue = $"<SID={value}>");
        }

        public bool Equals(SidName other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var casted = obj as SidName;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}