using System.DirectoryServices;
using ActiveDs;
using JetBrains.Annotations;
using static Bstm.DirectoryServices.DirectoryProperty;

namespace Bstm.DirectoryServices
{
    internal class User : DirectoryObject, IUser
    {
        public User([NotNull] DirectoryEntry directoryEntry) : base(directoryEntry)
        {
            MemberOf = new MemberOfCollection(this);
        }

        public bool AccountDisabled
        {
            get => HasAccountControl(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);

            set
            {
                if (value)
                {
                    SetAccountControl(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);
                }
                else
                {
                    ClearAccountControl(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);
                }
            }
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
            get => GetPropertyValue<string>(Mail);
            set => SetPropertyValue(Mail, value);
        }

        public string EmployeeId
        {
            get => GetPropertyValue<string>(DirectoryProperty.EmployeeId);
            set => SetPropertyValue(DirectoryProperty.EmployeeId, value);
        }

        public string FaxNumber
        {
            get => GetPropertyValue<string>(FacsimileTelephoneNumber);
            set => SetPropertyValue(FacsimileTelephoneNumber, value);
        }

        public string FirstName
        {
            get => GetPropertyValue<string>(GivenName);
            set => SetPropertyValue(GivenName, value);
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
            get => GetPropertyValue<string>(WwwHomePage);
            set => SetPropertyValue(WwwHomePage, value);
        }

        public IMemberOfCollection MemberOf { get; }

        internal bool HasAccountControl(ADS_USER_FLAG flag)
        {
            return GetPropertyValue<ADS_USER_FLAG>(UserAccountControl).HasFlag(flag);
        }

        internal void SetAccountControl(ADS_USER_FLAG flag)
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            var newValue = GetPropertyValue<ADS_USER_FLAG>(UserAccountControl) | flag;
            SetPropertyValue(UserAccountControl, newValue);
        }

        private void ClearAccountControl(ADS_USER_FLAG flag)
        {
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            var newValue = GetPropertyValue<ADS_USER_FLAG>(UserAccountControl) & ~flag;
            SetPropertyValue(UserAccountControl, newValue);
        }
    }
}