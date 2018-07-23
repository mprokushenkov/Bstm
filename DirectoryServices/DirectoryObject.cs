using System;
using System.DirectoryServices;
using Bstm.Common;
using JetBrains.Annotations;
using NLog;

namespace Bstm.DirectoryServices
{
    internal class DirectoryObject : IDirectoryObject //-V3073
    {
        private OrganizationalUnit parent;

        public DirectoryObject([NotNull] DirectoryEntry directoryEntry)
        {
            DirectoryEntry = Guard.CheckNull(directoryEntry, nameof(directoryEntry));
            Properties = new PropertyValueCollection(DirectoryEntry);
        }

        public Guid Guid => DirectoryEntry.Guid;

        public string Name => DistinguishedName.AddressableObjectName.Value;

        public string DisplayName
        {
            get => GetPropertyValue<string>(DirectoryProperty.DisplayName);
            set => SetPropertyValue(DirectoryProperty.DisplayName, Guard.CheckNull(value, nameof(value)));
        }

        // ReSharper disable once AssignNullToNotNullAttribute
        public DN DistinguishedName => GetPropertyValue<DN>(DirectoryProperty.DistinguishedName);

        public AdsPath Path => AdsPath.Parse(DirectoryEntry.Path);

        public PropertyValueCollection Properties { get; }

        public IOrganizationalUnit Parent => parent ?? (parent = new OrganizationalUnit(DirectoryEntry.Parent));

        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected internal DirectoryEntry DirectoryEntry { get; }

        public T GetPropertyValue<T>(DirectoryProperty directoryProperty)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            return Properties.GetValue<T>(directoryProperty);
        }

        public void SetPropertyValue(DirectoryProperty directoryProperty, object value)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            Properties.SetValue(directoryProperty, value);
        }

        public void Save()
        {
            Properties.Flush();
            DirectoryEntry.CommitChanges();
        }

        public void Rename(string newName)
        {
            Guard.CheckNull(newName, nameof(newName));

            var newRdn = new Rdn(DistinguishedName.AddressableObjectName.NamingAttribute, newName);

            DirectoryEntry.Rename(newRdn);
            Save();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return DistinguishedName;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DirectoryEntry.Dispose();
            }
        }
    }
}