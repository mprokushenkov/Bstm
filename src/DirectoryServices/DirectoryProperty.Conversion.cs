using System;
using System.Linq;
using ActiveDs;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public partial class DirectoryProperty
    {
        [NotNull]
        public string CreateSearchFilterString([NotNull] object value)
        {
            Guard.CheckNull(value, nameof(value));

            switch (value)
            {
                case Guid g:
                    return string.Concat(g.ToByteArray().Select(b => $"\\{b:x2}"));
                default:
                    return value.ToString();
            }
        }

        [NotNull]
        public object ConvertToDirectoryValue([NotNull] object value)
        {
            Guard.CheckNull(value, nameof(value));

            switch (value)
            {
                case Guid g:
                    return g.ToByteArray();
                case DN dn:
                    return dn.ToString();
                case ADS_USER_FLAG flag:
                    return (int) flag;
                case DateTimeOffset offset:
                    return DateTimeOffsetToLargeInteger(offset);
                case long l when DirectoryType == typeof(IADsLargeInteger):
                    return Int64ToLargeInteger(l);
                default:
                    return value;
            }
        }

        public object ConvertFromDirectoryValue([CanBeNull] object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                switch (value)
                {
                    case byte[] ar when NotionalType == typeof(Guid):
                        return new Guid(ar);
                    case string s when NotionalType == typeof(DN):
                        return DN.Parse(s);
                    case int i when NotionalType == typeof(ADS_USER_FLAG):
                        return (ADS_USER_FLAG) i;
                    case IADsLargeInteger i when NotionalType == typeof(DateTimeOffset?):
                        return DateTimeOffsetFromLargeInteger(i);
                    case IADsLargeInteger i when NotionalType == typeof(long):
                        return Int64FromLargeInteger(i);
                    default:
                        return value;
                }
            }
            catch (Exception e)
            {
                var message = string.Format(
                    ErrorMessages.DirectoryProperty_DirectoryServicesException_InappropriateDirectoryValue,
                    value,
                    value.GetType(),
                    Syntax,
                    Name);

                throw new DirectoryServicesException(message, e);
            }
        }

        internal static DateTimeOffset? DateTimeOffsetFromLargeInteger(object largeInteger)
        {
            try
            {
                return DateTimeOffset.FromFileTime(Int64FromLargeInteger(largeInteger));
            }
            catch (ArgumentOutOfRangeException)
            {
                // in case of very big integer
                return null;
            }
        }

        internal static IADsLargeInteger DateTimeOffsetToLargeInteger(DateTimeOffset offset)
        {
            try
            {
                return Int64ToLargeInteger(offset.ToFileTime());
            }
            catch (ArgumentOutOfRangeException)
            {
                // in case of very small offset 
                return new LargeInteger();
            }
        }

        internal static long Int64FromLargeInteger(object o)
        {
            var largeInt = (IADsLargeInteger) o;
            return ((long) largeInt.HighPart << 32) | (uint) largeInt.LowPart;
        }

        internal static IADsLargeInteger Int64ToLargeInteger(long val)
        {
            var largeInt = new LargeInteger
            {
                HighPart = (int) (val >> 32),
                LowPart = (int) val
            };

            return largeInt;
        }
    }
}