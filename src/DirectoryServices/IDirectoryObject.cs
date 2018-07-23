using System;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IDirectoryObject : IDisposable
    {
        Guid Guid { get; }

        [NotNull]
        string Name { get; }

        [CanBeNull]
        string DisplayName { get; set; }

        [NotNull]
        DN DistinguishedName { get; }

        [NotNull]
        AdsPath Path { get; }

        [NotNull]
        PropertyValueCollection Properties { get; }

        [NotNull]
        IOrganizationalUnit Parent { get; }

        void Save();

        void Rename([NotNull] string newName);

        [CanBeNull]
        T GetPropertyValue<T>([NotNull] DirectoryProperty directoryProperty);

        void SetPropertyValue([NotNull] DirectoryProperty directoryProperty, [CanBeNull] object value);
    }
}