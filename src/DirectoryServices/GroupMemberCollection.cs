using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal abstract class DirectoryObjectCollection : IEnumerable<IDirectoryObject>
    {
        private readonly IDirectoryObject owner;

        protected DirectoryObjectCollection([NotNull] IDirectoryObject group)
        {
            this.owner = Guard.CheckNull(group, nameof(group));
        }

        protected abstract DirectoryProperty DNCollectionProperty { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IDirectoryObject> GetEnumerator()
        {
            foreach (var dn in owner.Properties.GetValues<DN>(DNCollectionProperty))
            {
                var adsPath = owner.Path.Server != null
                    ? new AdsPath(owner.Path.Provider, owner.Path.Server, dn)
                    : new AdsPath(owner.Path.Provider, dn);

                DirectoryEntry directoryEntry = null;
                DirectoryObject directoryObject = null;

                try
                {
                    directoryEntry = new DirectoryEntry(adsPath);
                    directoryObject = directoryEntry.ToDirectoryObject();

                    var result = directoryObject;
                    directoryEntry = null;
                    directoryObject = null;

                    yield return result;
                }
                finally
                {
                    directoryEntry?.Dispose();
                    directoryObject?.Dispose();
                }
            }
        }
    }

    internal class GroupMemberCollection : DirectoryObjectCollection, IGroupMemberCollection //-V3072
    {
        private readonly IGroup group;

        public GroupMemberCollection(IGroup group) : base(group)
        {
            this.group = group;
        }

        protected override DirectoryProperty DNCollectionProperty { get; } = DirectoryProperty.Member;

        public void Add(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            group.Properties.AppendValue(DirectoryProperty.Member, directoryObject.DistinguishedName.ToString());
        }

        public void Remove(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            group.Properties.RemoveValue(DirectoryProperty.Member, directoryObject.DistinguishedName.ToString());
        }
    }
}