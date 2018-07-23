using Bstm.Common;

namespace Bstm.DirectoryServices
{
    internal class SchemaClassName : Enumeration
    {
        protected SchemaClassName(string name) : base(name)
        {
        }

        public static SchemaClassName Group { get; } = new SchemaClassName("group");

        public static SchemaClassName User { get; } = new SchemaClassName("user");

        public static SchemaClassName OrganizationalUnit { get; } = new SchemaClassName("organizationalUnit");
    }
}