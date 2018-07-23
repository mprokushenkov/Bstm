using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using Bstm.Common;

namespace Bstm.DirectoryServices
{
    internal class GroupMembersCollection : IGroupMembersCollection //-V3072
    {
        private readonly IGroup owner;

        public GroupMembersCollection(IGroup owner)
        {
            this.owner = Guard.CheckNull(owner, nameof(owner));
        }

        public void Add(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            owner.Properties.AppendValue(DirectoryProperty.Member, directoryObject.DistinguishedName.ToString());
        }

        public void Remove(IDirectoryObject directoryObject)
        {
            Guard.CheckNull(directoryObject, nameof(directoryObject));

            owner.Properties.RemoveValue(DirectoryProperty.Member, directoryObject.DistinguishedName.ToString());
        }

        public IEnumerator<IDirectoryObject> GetEnumerator()
        {
            foreach (var memberDN in owner.Properties.GetValues<DN>(DirectoryProperty.Member))
            {
                var adsPath = owner.Path.Server != null
                    ? new AdsPath(owner.Path.Provider, owner.Path.Server, memberDN)
                    : new AdsPath(owner.Path.Provider, memberDN);

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}