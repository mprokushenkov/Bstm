using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class MemberOfCollection : DirectoryObjectCollection<IGroup>, IMemberOfCollection
    {
        public MemberOfCollection([NotNull] IUser user) : base(Guard.CheckNull(user, nameof(user)))
        {
        }

        protected override DirectoryProperty DNCollectionProperty { get; } = DirectoryProperty.MemberOf;
    }
}