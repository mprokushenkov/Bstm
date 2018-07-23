using System.DirectoryServices;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal static class DirectoryEntryExtensions
    {
        [NotNull]
        public static DirectoryObject ToDirectoryObject([NotNull] this DirectoryEntry entry)
        {
            Guard.CheckNull(entry, nameof(entry));

            if (entry.SchemaClassName == SchemaClassName.Group)
            {
                return new Group(entry);
            }

            if (entry.SchemaClassName == SchemaClassName.User)
            {
                return new User(entry);
            }

            if (entry.SchemaClassName == SchemaClassName.OrganizationalUnit)
            {
                return new OrganizationalUnit(entry);
            }

            return new DirectoryObject(entry);
        }
    }
}