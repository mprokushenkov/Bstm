using Bstm.Common;

namespace Bstm.DirectoryServices
{
    public class AdsProvider : Enumeration
    {
        protected AdsProvider(string name) : base(name)
        {
        }

        public static AdsProvider Ldap { get; } = new AdsProvider("LDAP");

        public static AdsProvider GC { get; } = new AdsProvider("GC");
    }
}