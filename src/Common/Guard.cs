using System;

namespace Bstm.Common
{
    public static class Guard
    {
        public static T CheckNull<T>(T value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }

            return value;
        }

        public static Guid CheckEmpty(Guid value, string message)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(null, message);
            }

            return value;
        }

        public static string CheckNullOrEmpty(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(null, message);
            }

            return value;
        }
    }
}