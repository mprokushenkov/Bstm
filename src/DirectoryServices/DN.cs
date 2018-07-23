using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class DN : IAdsObjectName, IEquatable<DN>
    {
        private static readonly List<Rdn> wellknownGenericContainers = new List<Rdn>
        {
            Rdn.Parse("CN=Users"),
            Rdn.Parse("CN=Builtin"),
            Rdn.Parse("CN=Computers"),
            Rdn.Parse("CN=ForeignSecurityPrincipals"),
            Rdn.Parse("CN=LostAndFound"),
            Rdn.Parse("CN=Managed Service Accounts"),
            Rdn.Parse("CN=Program Data"),
            Rdn.Parse("CN=System"),
            Rdn.Parse("CN=NTDS Quotas"),
            Rdn.Parse("CN=TPM Devices"),
            Rdn.Parse("CN=Infrastructure")
        };

        private static readonly Regex splitRegex = new Regex(@"(?<!\\)\,", RegexOptions.Compiled);
        private readonly List<Rdn> rdns = new List<Rdn>();
        private string cachedStringValue;
        private Rdn addressableObjectName;
        private DN parent;
        private DN domain;
        private string fqdn;

        public DN([NotNull] params Rdn[] rdns)
        {
            Guard.CheckNull(rdns, nameof(rdns));

            if (!rdns.Any())
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    ErrorMessages.DN_ArgumentOutOfRangeException_EmptyRdnList);
            }

            CheckRdnSequence(rdns);

            this.rdns.AddRange(rdns);
        }

        [NotNull]
        public IEnumerable<Rdn> Rdns => rdns;

        [NotNull]
        public Rdn AddressableObjectName => addressableObjectName ?? (addressableObjectName = rdns.First());

        [CanBeNull]
        public DN Parent => parent ?? (parent = rdns.Count != 1 ? new DN(rdns.Skip(1).ToArray()) : null);

        [CanBeNull]
        public DN Domain
        {
            get
            {
                return domain ?? (domain = rdns.Any(rdn => rdn.NamingAttribute == NamingAttribute.Dc)
                           ? new DN(rdns.Where(rdn => rdn.NamingAttribute == NamingAttribute.Dc).ToArray())
                           : null);
            }
        }

        public string Fqdn
        {
            get
            {
                if (Domain == null)
                {
                    return null;
                }

                return fqdn ?? (fqdn = string.Join(".", Domain.Rdns.Select(rdn => rdn.Value)));
            }
        }

        [NotNull]
        public static DN Parse([NotNull] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    ErrorMessages.DN_ArgumentOutOfRangeException_InvalidDNValue);
            }

            Rdn[] rdns;

            try
            {
                rdns = splitRegex.Split(value).Select(rdn => Rdn.Parse(rdn.Trim())).ToArray();
            }
            catch (ArgumentOutOfRangeException e)
            {
                var message = string.Format(ErrorMessages.DN_ArgumentOutOfRangeException_InvalidParseValue, value);
                throw new ArgumentOutOfRangeException(message, e);
            }

            return new DN(rdns);
        }

        public static bool TryParse([NotNull] string value, out DN dn)
        {
            try
            {
                dn = Parse(value);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                dn = null;
                return false;
            }
        }

        [NotNull]
        public static DN FromFqdn([NotNull] string fqdn)
        {
            if (string.IsNullOrWhiteSpace(fqdn))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    ErrorMessages.DN_ArgumentOutOfRangeException_InvalidFqdnValue);
            }

            var rdns = fqdn.Split('.').Select(s => new Rdn(NamingAttribute.Dc, new LdapName(s))).ToArray();

            return new DN(rdns);
        }

        public static implicit operator string(DN dn)
        {
            return dn?.ToString();
        }

        public static bool operator ==(DN left, DN right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DN left, DN right)
        {
            return !Equals(left, right);
        }

        [NotNull]
        public DN Append([NotNull] params Rdn[] rdns)
        {
            Guard.CheckNull(rdns, nameof(rdns));

            var result = new DN(this.rdns.Concat(rdns).ToArray());

            return result;
        }

        [NotNull]
        public DN Append([NotNull] DN dn)
        {
            Guard.CheckNull(dn, nameof(dn));

            var result = new DN(rdns.Concat(dn.rdns).ToArray());

            return result;
        }

        [NotNull]
        public DN Prepend([NotNull] params Rdn[] rdns)
        {
            Guard.CheckNull(rdns, nameof(rdns));

            var result = new DN(rdns.Concat(this.rdns).ToArray());

            return result;
        }

        [NotNull]
        public DN Prepend([NotNull] DN dn)
        {
            Guard.CheckNull(dn, nameof(dn));

            var result = new DN(dn.rdns.Concat(rdns).ToArray());

            return result;
        }

        public bool Equals(DN other)
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

            var casted = obj as DN;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return cachedStringValue ?? (cachedStringValue = string.Join(",", rdns));
        }

        private static void CheckRdnSequence(params Rdn[] rdns)
        {
            if (IsRdnSequenceConsistent(rdns))
            {
                return;
            }

            var message = string.Format(
                ErrorMessages.DN_ArgumentOutOfRangeException_InvalidRdnSequence,
                string.Join(",", rdns.ToList()));

            throw new ArgumentOutOfRangeException(null, message);
        }

        private static bool IsRdnSequenceConsistent(Rdn[] rdns)
        {
            for (var i = 0; i < rdns.Length - 1; i++)
            {
                var current = rdns[i];
                var next = rdns[i + 1];

                if (current.NamingAttribute == NamingAttribute.Cn
                    && next.NamingAttribute != NamingAttribute.Ou
                    && !wellknownGenericContainers.Contains(current)
                    && !wellknownGenericContainers.Contains(next))
                {
                    return false;
                }

                if (current.NamingAttribute == NamingAttribute.Ou && next.NamingAttribute != NamingAttribute.Ou && next.NamingAttribute != NamingAttribute.Dc)
                {
                    return false;
                }

                if (current.NamingAttribute == NamingAttribute.Dc && next.NamingAttribute != NamingAttribute.Dc)
                {
                    return false;
                }
            }

            return true;
        }
    }
}