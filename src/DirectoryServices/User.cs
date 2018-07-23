using System.DirectoryServices;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    internal class User : DirectoryObject, IUser
    {
        public User([NotNull] DirectoryEntry directoryEntry) : base(directoryEntry)
        {

        }

        public string Department
        {
            get => GetPropertyValue<string>(DirectoryProperty.Department);
            set => SetPropertyValue(DirectoryProperty.Department, value);
        }

        public string Description
        {
            get => GetPropertyValue<string>(DirectoryProperty.Description);
            set => SetPropertyValue(DirectoryProperty.Description, value);
        }

        public string Division
        {
            get => GetPropertyValue<string>(DirectoryProperty.Division);
            set => SetPropertyValue(DirectoryProperty.Division, value);
        }

        public string EmailAddress
        {
            get => GetPropertyValue<string>(DirectoryProperty.Mail);
            set => SetPropertyValue(DirectoryProperty.Mail, value);
        }

        public string EmployeeId
        {
            get => GetPropertyValue<string>(DirectoryProperty.EmployeeId);
            set => SetPropertyValue(DirectoryProperty.EmployeeId, value);
        }

        public string FaxNumber
        {
            get => GetPropertyValue<string>(DirectoryProperty.FacsimileTelephoneNumber);
            set => SetPropertyValue(DirectoryProperty.FacsimileTelephoneNumber, value);
        }

        public string FirstName
        {
            get => GetPropertyValue<string>(DirectoryProperty.GivenName);
            set => SetPropertyValue(DirectoryProperty.GivenName, value);
        }

        public string FullName
        {
            get => GetPropertyValue<string>(DirectoryProperty.DisplayName);
            set => SetPropertyValue(DirectoryProperty.DisplayName, value);
        }

        public string HomeDirectory
        {
            get => GetPropertyValue<string>(DirectoryProperty.HomeDirectory);
            set => SetPropertyValue(DirectoryProperty.HomeDirectory, value);
        }

        public string HomePage
        {
            get => GetPropertyValue<string>(DirectoryProperty.WwwHomePage);
            set => SetPropertyValue(DirectoryProperty.WwwHomePage, value);
        }
    }
}