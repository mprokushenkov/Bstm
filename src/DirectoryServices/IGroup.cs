using System.DirectoryServices.AccountManagement;

namespace Bstm.DirectoryServices
{
    public interface IGroup : IDirectoryObject
    {
        GroupScope GroupScope { get; set; }

        IGroupMembersCollection Members { get; }
    }
}