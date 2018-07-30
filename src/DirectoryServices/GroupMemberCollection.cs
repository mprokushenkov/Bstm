using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class GroupMemberCollection : DirectoryObjectCollection<IDirectoryObject>, IGroupMemberCollection //-V3072
    {
        private readonly IGroup group;

        public GroupMemberCollection([NotNull] IGroup group) : base(group)
        {
            this.group = Guard.CheckNull(group, nameof(group));
        }

        protected override DirectoryProperty DNCollectionProperty { get; } = DirectoryProperty.Member;

        public void Add(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            group.Properties.AppendValue(DirectoryProperty.Member, directoryObject.DistinguishedName);
        }

        public void Remove(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            group.Properties.RemoveValue(DirectoryProperty.Member, directoryObject.DistinguishedName);
        }
    }
}