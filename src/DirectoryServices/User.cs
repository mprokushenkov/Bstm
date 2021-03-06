﻿using System;
using System.DirectoryServices;
using ActiveDs;
using JetBrains.Annotations;
using static Bstm.DirectoryServices.DirectoryProperty;
using static ActiveDs.ADS_USER_FLAG;

namespace Bstm.DirectoryServices
{
    internal class User : DirectoryObject, IUser
    {
        private bool managerInitialized;
        private IUser manager;

        public User([NotNull] DirectoryEntry directoryEntry) : base(directoryEntry)
        {
            MemberOf = new MemberOfCollection(this);
            SeeAlso = new DirectoryObjectCollection(this, DirectoryProperty.SeeAlso);
        }

        public bool AccountDisabled
        {
            get => HasUserFlag(ADS_UF_ACCOUNTDISABLE);

            set
            {
                if (value)
                {
                    AppendUserFlag(ADS_UF_ACCOUNTDISABLE);
                }
                else
                {
                    RemoveUserFlag(ADS_UF_ACCOUNTDISABLE);
                }
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

        public bool AccountLocked => GetPropertyValue<long?>(LockoutTime) > 0;

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

        public IUser Manager
        {
            get
            {
                if (!managerInitialized)
                {
                    InitializeManager();
                }

                return manager;
            }
            set
            {
                if (value != null)
                {
                    SetPropertyValue(DirectoryProperty.Manager, value.DistinguishedName);
                }
                else
                {
                    Properties.RemoveValue(DirectoryProperty.Manager, manager.DistinguishedName);
                }

                manager = value;
                managerInitialized = true;
            }
        }

        public long? MaxStorage
        {
            get => GetPropertyValue<long?>(DirectoryProperty.MaxStorage);
            set => SetPropertyValue(DirectoryProperty.MaxStorage, value);
        }

        public string NamePrefix
        {
            get => GetPropertyValue<string>(PersonalTitle);
            set => SetPropertyValue(PersonalTitle, value);
        }

        public string NameSuffix
        {
            get => GetPropertyValue<string>(GenerationQualifier);
            set => SetPropertyValue(GenerationQualifier, value);
        }

        public string OfficeLocation
        {
            get => GetPropertyValue<string>(PhysicalDeliveryOfficeName);
            set => SetPropertyValue(PhysicalDeliveryOfficeName, value);
        }

        public string OtherName
        {
            get => GetPropertyValue<string>(MiddleName);
            set => SetPropertyValue(MiddleName, value);
        }

        public DateTime? PasswordLastChanged => GetPropertyValue<DateTime?>(PwdLastSet);

        public bool PasswordRequired
        {
            get => HasUserFlag(ADS_UF_PASSWD_NOTREQD);

            set
            {
                if (value)
                {
                    AppendUserFlag(ADS_UF_PASSWD_NOTREQD);
                }
                else
                {
                    RemoveUserFlag(ADS_UF_PASSWD_NOTREQD);
                }
            }
        }

        public string PostalAddress
        {
            get => GetPropertyValue<string>(DirectoryProperty.PostalAddress);
            set => SetPropertyValue(DirectoryProperty.PostalAddress, value);
        }

        public string PostalCode
        {
            get => GetPropertyValue<string>(DirectoryProperty.PostalCode);
            set => SetPropertyValue(DirectoryProperty.PostalCode, value);
        }

        public string ProfilePath
        {
            get => GetPropertyValue<string>(DirectoryProperty.ProfilePath) ?? string.Empty;
            set => SetPropertyValue(DirectoryProperty.ProfilePath, value);
        }

        private void InitializeManager()
        {
            var managerPath = CreateManagerPath();

            if (managerPath != null)
            {
                manager = new User(new DirectoryEntry(managerPath));
            }

            managerInitialized = true;
        }

        private AdsPath CreateManagerPath()
        {
            var dn = GetPropertyValue<DN>(DirectoryProperty.Manager);

            if (dn == null)
            {
                return null;
            }

            var adsPath = Path.Server != null
                ? new AdsPath(Path.Provider, Path.Server, dn)
                : new AdsPath(Path.Provider, dn);

            return adsPath;
        }

        public IMemberOfCollection MemberOf { get; }

        public IDirectoryObjectCollection SeeAlso { get; }

        private bool HasUserFlag(ADS_USER_FLAG flag) =>
            GetPropertyValue<ADS_USER_FLAG>(UserAccountControl).HasFlag(flag);

        private void AppendUserFlag(ADS_USER_FLAG flag)
        {
            // ReSharper disable BitwiseOperatorOnEnumWithoutFlags

            var currentValue = GetPropertyValue<ADS_USER_FLAG>(UserAccountControl);
            SetPropertyValue(UserAccountControl, currentValue | flag);

            // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
        }

        private void RemoveUserFlag(ADS_USER_FLAG flag)
        {
            // ReSharper disable BitwiseOperatorOnEnumWithoutFlags

            var currentValue = GetPropertyValue<ADS_USER_FLAG>(UserAccountControl);
            SetPropertyValue(UserAccountControl, currentValue & ~flag);

            // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
        }
    }
}