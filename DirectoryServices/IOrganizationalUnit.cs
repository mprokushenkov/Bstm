using System.Collections.Generic;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IOrganizationalUnit : IDirectoryObject
    {
        [NotNull]
        IReadOnlyCollection<IDirectoryObject> Children { get; }

        void RemoveChild([NotNull] IDirectoryObject child);

        IGroup CreateGroup([NotNull] LdapName name);

        IUser CreateUser([NotNull] LdapName name);

        IOrganizationalUnit CreateOrganizationalUnit([NotNull] LdapName name);
    }
}