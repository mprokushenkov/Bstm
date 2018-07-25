﻿using System.DirectoryServices.AccountManagement;

namespace Bstm.DirectoryServices
{
    public interface IGroup : IDirectoryObject
    {
        GroupScope Scope { get; set; }

        IGroupMembersCollection Members { get; }
    }
}