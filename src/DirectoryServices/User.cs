using System;
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
            get => GetPropertyValue<ADS_USER_FLAG>(UserAccountControl).HasFlag(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);

            set
            {
                // ReSharper disable BitwiseOperatorOnEnumWithoutFlags

                var currentValue = GetPropertyValue<ADS_USER_FLAG>(UserAccountControl);

                if (value)
                {
                    SetPropertyValue(UserAccountControl, currentValue | ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);
                }
                else
                {
                    SetPropertyValue(UserAccountControl, currentValue & ~ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);
                }

                // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
            }
        }

        public DateTime? AccountExpires
        {
            get => GetPropertyValue<DateTime?>(DirectoryProperty.AccountExpires);
            set => SetPropertyValue(DirectoryProperty.AccountExpires, value);
        }

        public int BadLoginCount => GetPropertyValue<int>(BadPwdCount);

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

        public bool AccountLocked => GetPropertyValue<long>(LockoutTime) > 0;

        public DateTime? LastFailedLogin => GetPropertyValue<DateTime?>(BadPasswordTime);

        public DateTime? LastLogin => GetPropertyValue<DateTime?>(LastLogon);

        public DateTime? LastLogoff => GetPropertyValue<DateTime?>(DirectoryProperty.LastLogoff);

        public string LastName
        {
            get => GetPropertyValue<string>(Sn);
            set => SetPropertyValue(Sn, value);
        }

        public string LoginScript
        {
            get => GetPropertyValue<string>(ScriptPath);
            set => SetPropertyValue(ScriptPath, value);
        }

        public IMemberOfCollection MemberOf { get; }
    }
}