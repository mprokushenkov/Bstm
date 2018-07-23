using System;
using System.Linq;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class Rdn : IEquatable<Rdn>
    {
        private readonly NamingAttribute namingAttribute;
        private readonly LdapName value;
        private string cachedStringValue;

        public Rdn([NotNull] NamingAttribute namingAttribute, [NotNull] LdapName name)
        {
            this.namingAttribute = Guard.CheckNull(namingAttribute, nameof(namingAttribute));
            value = Guard.CheckNull(name, nameof(name));
        }

        [NotNull]
        public NamingAttribute NamingAttribute => namingAttribute;

        [NotNull]
        public LdapName Value => value;

        public static implicit operator string(Rdn rdn)
        {
            return rdn?.ToString();
        }

        public static bool operator ==(Rdn left, Rdn right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Rdn left, Rdn right)
        {
            return !Equals(left, right);
        }

        [NotNull]
        public static Rdn Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    ErrorMessages.Rdn_ArgumentOutOfRangeException_InvalidValue);
            }

            var segments = value.Split('=');

            if (segments.Length != 2)
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    string.Format(ErrorMessages.Rdn_ArgumentOutOfRangeException_InvalidParseValue, value));
            }

            var namingAttribute = Enumeration.FromName<NamingAttribute>(segments.First());
            var name = new LdapName(segments.Last());

            return new Rdn(namingAttribute, name);
        }

        public static bool TryParse([NotNull] string value, out Rdn rdn)
        {
            try
            {
                rdn = Parse(value);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                rdn = null;
                return false;
            }
        }

        public bool Equals(Rdn other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(ToString(), other.ToString());
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

            var casted = obj as Rdn;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return cachedStringValue ?? (cachedStringValue = $"{namingAttribute}={value}");
        }
    }
}