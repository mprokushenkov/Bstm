using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Bstm.Common;
using JetBrains.Annotations;
using NLog;

namespace Bstm.DirectoryServices
{
    public sealed class PropertyValueCollection //-V3072
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly string[] propertiesProhibitedForUpdate =
            {DirectoryProperty.DistinguishedName};

        private readonly DirectoryEntry directoryEntry;
        private readonly List<(string propertyName, object propertyValue)> store = new List<(string, object)>();
        private readonly List<DirectoryProperty> removedProperties = new List<DirectoryProperty>();

        [UsedImplicitly]
        public PropertyValueCollection()
        {
        }

        internal PropertyValueCollection(DirectoryEntry directoryEntry)
        {
            this.directoryEntry = Guard.CheckNull(directoryEntry, nameof(directoryEntry));
        }

        [NotNull]
        public object[] this[[NotNull] DirectoryProperty directoryProperty]
        {
            get
            {
                Guard.CheckNull(directoryProperty, nameof(directoryProperty));

                return GetValues<object>(directoryProperty);
            }
        }

        [CanBeNull]
        public T GetValue<T>([NotNull] DirectoryProperty directoryProperty)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            return GetValues<T>(directoryProperty).FirstOrDefault();
        }

        [NotNull]
        public T[] GetValues<T>([NotNull] DirectoryProperty directoryProperty)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            var values = GetValuesFromStore<T>(directoryProperty);

            if (values.Length != 0)
            {
                return values;
            }

            if (directoryEntry == null)
            {
                return values;
            }

            UpdateValuesInStore(directoryProperty, directoryEntry.Properties[directoryProperty].Cast<object>().ToArray());

            return GetValuesFromStore<T>(directoryProperty);
        }

        public void SetValue([NotNull] DirectoryProperty directoryProperty, [CanBeNull] object value)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            SetValues(directoryProperty, new[] {value});
        }

        public void SetValues([NotNull] DirectoryProperty directoryProperty, [NotNull] object[] values)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));
            Guard.CheckNull(values, nameof(values));

            RemoveValues(directoryProperty);
            UpdateValuesInStore(directoryProperty, values);
            RemoveFromRemovedProperties(directoryProperty);
        }

        public void RemoveValue(DirectoryProperty directoryProperty, [CanBeNull] object value)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            var itemsForRemove = store
                .Where(t => t.propertyName == directoryProperty && t.propertyValue.GetHashCode() == value?.GetHashCode())
                .ToArray();

            if (itemsForRemove.Length == 0)
            {
                return;
            }

            itemsForRemove.ForEach(item => store.Remove(item));

            AddToRemovedProperties(directoryProperty);
        }

        public void RemoveValues(DirectoryProperty directoryProperty)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            var itemsForRemove = store.Where(t => t.propertyName == directoryProperty).ToArray();
            itemsForRemove.ForEach(item => store.Remove(item));

            AddToRemovedProperties(directoryProperty);
        }

        public void AppendValue(DirectoryProperty directoryProperty, [CanBeNull] object value)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));

            AppendValues(directoryProperty, new[] {value});
        }

        public void AppendValues(DirectoryProperty directoryProperty, [NotNull] object[] values)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));
            Guard.CheckNull(values, nameof(values));

            UpdateValuesInStore(directoryProperty, values);
            RemoveFromRemovedProperties(directoryProperty);
        }

        public void Flush()
        {
            store.ToLookup(t => t.propertyName).ForEach(group =>
            {
                UpdateDirectoryEntry(group.Key, group.Select(t => t.propertyValue).ToArray());
            });
        }

        public void Refresh([NotNull] DirectoryProperty[] properties)
        {
            Guard.CheckNull(properties, nameof(properties));

            Clear();
            properties.ForEach(p => GetValue<object>(p));
        }

        internal bool ValueHasBeenRemoved(DirectoryProperty property)
        {
            return removedProperties.Any(p => p == property);
        }

        internal void Clear()
        {
            store.Clear();
        }

        private void AddToRemovedProperties(DirectoryProperty directoryProperty)
        {
            if (store.Any(t => t.propertyName == directoryProperty))
            {
                return;
            }

            removedProperties.Add(directoryProperty);
        }

        private void RemoveFromRemovedProperties(DirectoryProperty directoryProperty)
        {
            var index = removedProperties.FindIndex(p => p == directoryProperty);

            if (index == -1)
            {
                return;
            }

            removedProperties.RemoveAt(index);
        }

        private T[] GetValuesFromStore<T>(DirectoryProperty directoryProperty)
        {
            var result = store
                .ToLookup(t => t.propertyName)[directoryProperty]
                .Select(t => directoryProperty.ConvertFromDirectoryValue(t.propertyValue))
                .OfType<T>().
                ToArray();

            return result;
        }

        private void UpdateValuesInStore(DirectoryProperty directoryProperty, object[] values)
        {
            Guard.CheckNull(directoryProperty, nameof(directoryProperty));
            Guard.CheckNull(values, nameof(values));

            var convertedValues = values.Select(directoryProperty.ConvertToDirectoryValue).ToArray();
            store.AddRange(values.Select(v => (directoryProperty.ToString(), v)));
        }

        private void UpdateDirectoryEntry(string propertyName, object[] values)
        {
            Guard.CheckNull(propertyName, nameof(propertyName));
            Guard.CheckNull(values, nameof(values));

            if (propertiesProhibitedForUpdate.Contains(propertyName))
            {
                return;
            }

            if (directoryEntry == null)
            {
                return;
            }

            logger.Debug(
                "Flush {DirectoryProperty} {Values} to underlying object.",
                propertyName,
                string.Join(", ", values));

            switch (values.Length)
            {
                case 0:
                    directoryEntry.Properties[propertyName].Clear();
                    return;
                case 1:
                    directoryEntry.Properties[propertyName].Value = values[0];
                    break;
                default:
                    directoryEntry.Properties[propertyName].Value = values;
                    break;
            }

            removedProperties.ForEach(p => directoryEntry.Properties[p].Clear());
        }
    }
}