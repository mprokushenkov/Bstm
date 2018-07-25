using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IGroupMemberCollection
    {
        void Add([NotNull] IDirectoryObject directoryObject);
        void Remove([NotNull] IDirectoryObject directoryObject);
    }
}