using System.Collections.Generic;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IGroupMemberCollection : IEnumerable<IDirectoryObject>
    {
        void Add([NotNull] IDirectoryObject directoryObject);
        void Remove([NotNull] IDirectoryObject directoryObject);
    }
}