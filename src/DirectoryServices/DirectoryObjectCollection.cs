using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class DirectoryObjectCollection : DirectoryObjectCollection<IDirectoryObject>, IDirectoryObjectCollection
    {
        public DirectoryObjectCollection(
            [NotNull] IDirectoryObject directoryObject,
            DirectoryProperty dnCollectionProperty) : base(Guard.CheckNull(directoryObject, nameof(directoryObject)))
        {
            DNCollectionProperty = Guard.CheckNull(dnCollectionProperty, nameof(dnCollectionProperty));
        }

        protected override DirectoryProperty DNCollectionProperty { get; }
    }
}