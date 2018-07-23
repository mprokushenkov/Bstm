using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class OrganizationalUnit : DirectoryObject, IOrganizationalUnit
    {
        private readonly Lazy<List<DirectoryObject>> children;

        public OrganizationalUnit([NotNull] DirectoryEntry directoryEntry) : base(directoryEntry)
        {
            children = new Lazy<List<DirectoryObject>>(CreateChildren);
        }

        public IReadOnlyCollection<IDirectoryObject> Children => children.Value;

        public void RemoveChild(IDirectoryObject child)
        {
            Guard.CheckNull(child, nameof(child));

            Logger.Debug("Delete directory object {Child} from {DN}.", child.Name, DistinguishedName);

            DirectoryEntry.Children.Remove(((DirectoryObject) child).DirectoryEntry);
            children.Value.Remove((DirectoryObject) child);

            Logger.Debug("Directory object {Child} successfully deleted from {DN}.", child.Name, DistinguishedName);
        }

        public IGroup CreateGroup(LdapName name)
        {
            Guard.CheckNull(name, nameof(name));

            Logger.Debug("Create group {Group} in {DN}.", name, DistinguishedName);

            var child = DirectoryEntry.Children.Add(
                new Rdn(NamingAttribute.Cn, name).ToString(),
                SchemaClassName.Group);

            child.Properties[DirectoryProperty.SamAccountName].Add(name.ToString());

            var group = new Group(child);
            group.Save();

            children.Value.Add(group);

            Logger.Debug("Group {Group} successfully created in {DN}.", name, DistinguishedName);

            return group;
        }

        public IUser CreateUser(LdapName name)
        {
            Guard.CheckNull(name, nameof(name));

            Logger.Debug("Create user {User} in {DN}.", name, DistinguishedName);

            var child = DirectoryEntry.Children.Add(
                new Rdn(NamingAttribute.Cn, name).ToString(),
                SchemaClassName.User);

            child.Properties[DirectoryProperty.SamAccountName].Add(name.ToString());

            var user = new User(child);
            user.Save();

            children.Value.Add(user);

            Logger.Debug("User {User} successfully created in {DN}.", name, DistinguishedName);

            return user;
        }

        public IOrganizationalUnit CreateOrganizationalUnit(LdapName name)
        {
            Guard.CheckNull(name, nameof(name));

            Logger.Debug("Create organizational unit {OrganizationalUnit} in {DN}.", name, DistinguishedName);

            var child = DirectoryEntry.Children.Add(
                new Rdn(NamingAttribute.Ou, name).ToString(),
                SchemaClassName.OrganizationalUnit);

            var ou = new OrganizationalUnit(child);
            ou.Save();

            children.Value.Add(ou);

            Logger.Debug(
                "Organizational unit {OrganizationalUnit} successfully created in {DN}.",
                name,
                DistinguishedName);

            return ou;
        }

        private List<DirectoryObject> CreateChildren()
        {
            var result = new List<DirectoryObject>(
                DirectoryEntry.Children.Cast<DirectoryEntry>().Select(e => e.ToDirectoryObject()));

            return result;
        }
    }
}