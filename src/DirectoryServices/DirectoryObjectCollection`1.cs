using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace Bstm.DirectoryServices
{
    internal abstract class DirectoryObjectCollection<T> : IEnumerable<T> where T : IDirectoryObject
    {
        private readonly IDirectoryObject owner;

        protected DirectoryObjectCollection(IDirectoryObject owner)
        {
            this.owner = owner;
        }

        protected abstract DirectoryProperty DNCollectionProperty { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var dn in owner.Properties.GetValues<DN>(DNCollectionProperty))
            {
                var adsPath = CreateAdsPath(dn);

                DirectoryEntry directoryEntry = null;
                DirectoryObject directoryObject = null;

                try
                {
                    directoryEntry = new DirectoryEntry(adsPath);
                    directoryObject = directoryEntry.ToDirectoryObject();

                    var result = directoryObject;

                    directoryEntry = null;
                    directoryObject = null;

                    yield return (T) (IDirectoryObject) result;
                }
                finally
                {
                    directoryEntry?.Dispose();
                    directoryObject?.Dispose();
                }
            }
        }

        private AdsPath CreateAdsPath(IAdsObjectName dn)
        {
            var adsPath = owner.Path.Server != null
                ? new AdsPath(owner.Path.Provider, owner.Path.Server, dn)
                : new AdsPath(owner.Path.Provider, dn);

            return adsPath;
        }
    }
}