using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Bstm.PowerShellServices
{
    [Serializable]
    public class PowerShellServicesException : Exception
    {
        public PowerShellServicesException()
        {
        }

        public PowerShellServicesException(string message)
            : base(message)
        {
        }

        public PowerShellServicesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PowerShellServicesException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}