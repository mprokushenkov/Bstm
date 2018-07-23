using Bstm.Common;

namespace Bstm.DirectoryServices
{
    public class NamingAttribute : Enumeration
    {
        protected NamingAttribute(string value) : base(value)
        {
        }

        public static NamingAttribute Cn { get; } = new NamingAttribute("CN");

        public static NamingAttribute Ou { get; } = new NamingAttribute("OU");

        public static NamingAttribute Dc { get; } = new NamingAttribute("DC");
    }
}