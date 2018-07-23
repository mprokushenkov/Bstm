using System;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class GuidName : IAdsObjectName, IEquatable<GuidName>
    {
        private readonly Guid value;
        private string cachedStringValue;

        public GuidName(Guid value)
        {
            this.value = value;
        }

        public static implicit operator string(GuidName name)
        {
            return name?.ToString();
        }

        public static bool operator ==(GuidName left, GuidName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GuidName left, GuidName right)
        {
            return !Equals(left, right);
        }

        [NotNull]
        public static GuidName Parse([NotNull] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    string.Format(ErrorMessages.GuidName_ArgumentOutOfRangeException_InvalidValue));
            }

            try
            {
                var candidate = value.Trim('<', '>').Replace("GUID=", string.Empty);
                return new GuidName(Guid.Parse(candidate));
            }
            catch (FormatException)
            {
                var message = string.Format(
                    ErrorMessages.GuidName_ArgumentOutOfRangeException_InvalidParseValue,
                    value);

                throw new ArgumentOutOfRangeException(null, message);
            }
        }

        public static bool TryParse([NotNull] string value, out GuidName guidName)
        {
            try
            {
                guidName = Parse(value);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                guidName = null;
                return false;
            }
        }

        public override string ToString()
        {
            return cachedStringValue ?? (cachedStringValue = $"<GUID={value}>");
        }

        public bool Equals(GuidName other)
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

            var casted = obj as GuidName;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}