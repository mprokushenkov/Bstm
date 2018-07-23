using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class Group : DirectoryObject, IGroup
    {
        public Group([NotNull] DirectoryEntry directoryEntry) : base(directoryEntry)
        {
            Members = new GroupMembersCollection(this);
        }

        public GroupScope GroupScope
        {
            get
            {
                var groupType = GetPropertyValue<int>(DirectoryProperty.GroupType);

                if ((groupType & 2) != 0)
                {
                    return GroupScope.Global;
                }

                if ((groupType & 4) != 0)
                {
                    return GroupScope.Local;
                }

                if ((groupType & 8) != 0)
                {
                    return GroupScope.Universal;
                }

                throw new InvalidOperationException("Invalid group type.");
            }
            set
            {
                var groupType = -2147483648; // security group

                switch (value)
                {
                    case GroupScope.Global:
                        groupType |= 2;
                        break;
                    case GroupScope.Local:
                        groupType |= 4;
                        break;
                    case GroupScope.Universal:
                        groupType |= 8;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                SetPropertyValue(DirectoryProperty.GroupType, groupType);
            }
        }

        public IGroupMembersCollection Members { get; }
    }
}