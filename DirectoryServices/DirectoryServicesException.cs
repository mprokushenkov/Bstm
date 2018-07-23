using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    [Serializable]
    public class DirectoryServicesException : Exception
    {
        public DirectoryServicesException()
        {
        }

        public DirectoryServicesException(string message)
            : base(message)
        {
        }

        public DirectoryServicesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DirectoryServicesException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}