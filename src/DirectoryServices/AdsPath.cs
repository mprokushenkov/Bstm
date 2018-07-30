using System;
using System.Text;
using System.Text.RegularExpressions;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class AdsPath : IEquatable<AdsPath>
    {
        private static readonly Regex parseExpression = new Regex(
            "^(?<provider>LDAP|GC)://(?<name>.+)",
            RegexOptions.Compiled);

        private string cachedStringValue;

        public AdsPath([NotNull] IAdsObjectName objectName)
        {
            ObjectName = Guard.CheckNull(objectName, nameof(objectName));
        }

        public AdsPath([NotNull] AdsProvider provider, [NotNull] IAdsObjectName objectName)
            : this(objectName)
        {
            Provider = Guard.CheckNull(provider, nameof(provider));
        }

        public AdsPath([NotNull] string server, [NotNull] IAdsObjectName objectName)
            : this(objectName)
        {
            Server = Guard.CheckNull(server, nameof(server));
        }

        public AdsPath([NotNull] AdsProvider provider, [NotNull] string server, [NotNull] IAdsObjectName objectName)
            : this(server, objectName)
        {
            Provider = Guard.CheckNull(provider, nameof(provider));
        }

        public static AdsPath RootDse { get; } = new AdsPath(new RootDseName());

        public AdsProvider Provider { get; }

        public string Server { get; }

        public IAdsObjectName ObjectName { get; }

        [NotNull]
        public static AdsPath Parse([NotNull] string path)
        {
            Guard.CheckNull(path, nameof(path));

            if (TryGetAdsPathWithServer(path, out var adsPath))
            {
                return adsPath;
            }

            if (TryGetAdsPathWithoutServer(path, out adsPath))
            {
                return adsPath;
            }

            if (TryGetAdsPathWithoutProviderAndServer(path, out adsPath))
            {
                return adsPath;
            }

            var message = string.Format(
                ErrorMessages.AdsPath_ArgumentOutOfRangeException_InvalidParseValue,
                path);

            throw new ArgumentOutOfRangeException(null, message);
        }

        public static bool TryParse([NotNull] string value, out AdsPath adsPath)
        {
            try
            {
                adsPath = Parse(value);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                adsPath = null;
                return false;
            }
        }

        public static implicit operator string(AdsPath path)
        {
            return path?.ToString();
        }

        public static bool operator ==(AdsPath left, AdsPath right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AdsPath left, AdsPath right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return cachedStringValue ?? (cachedStringValue = CreateStringValue());
        }

        public bool Equals(AdsPath other)
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

            var casted = obj as AdsPath;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        private string CreateStringValue()
        {
            var sb = new StringBuilder($"{Provider ?? AdsProvider.Ldap}://");
            sb.Append(Server ?? string.Empty);

            if (Server != null)
            {
                sb.Append("/");
            }

            sb.Append(ObjectName);

            return sb.ToString();
        }

        private static bool TryGetAdsPathWithServer(string path, out AdsPath adsPath)
        {
            try
            {
                var uri = new Uri(path);

                var provider = Enumeration.FromName<AdsProvider>(uri.Scheme.ToUpperInvariant());
                var server = uri.Host;
                var objectName = DN.Parse(uri.LocalPath.Substring(1));
                adsPath = new AdsPath(provider, server, objectName);

                return true;
            }
            catch (UriFormatException)
            {
                adsPath = null;
                return false;
            }
        }

        private static bool TryGetAdsPathWithoutServer(string path, out AdsPath adsPath)
        {
            var match = parseExpression.Match(path);

            if (!match.Success)
            {
                adsPath = null;
                return false;
            }

            try
            {
                var provider = Enumeration.FromName<AdsProvider>(match.Groups["provider"].Value);
                var objectName = GetObjectNameFromString(match.Groups["name"].Value);
                adsPath = new AdsPath(provider, objectName);

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                adsPath = null;
                return false;
            }
        }

        private static bool TryGetAdsPathWithoutProviderAndServer(string path, out AdsPath adsPath)
        {
            try
            {
                var objectName = GetObjectNameFromString(path);
                adsPath = new AdsPath(objectName);

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                adsPath = null;
                return false;
            }
        }

        private static IAdsObjectName GetObjectNameFromString(string value)
        {
            if (DN.TryParse(value, out var dn))
            {
                return dn;
            }

            if (GuidName.TryParse(value, out var guidName))
            {
                return guidName;
            }

            if (SidName.TryParse(value, out var sidName))
            {
                return sidName;
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        private struct RootDseName : IAdsObjectName
        {
            public override string ToString()
            {
                return "RootDSE";
            }
        }
    }
}