﻿using System;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IUser : IDirectoryObject
    {
        bool AccountDisabled { get; set; }

        [CanBeNull]
        DateTime? AccountExpires { get; set; }

        int BadLoginCount { get; }

        [CanBeNull]
        string Department { get; set; }

        [CanBeNull]
        string Description { get; set; }

        [CanBeNull]
        string Division { get; set; }

        [CanBeNull]
        string EmailAddress { get; set; }

        [CanBeNull]
        string EmployeeId { get; set; }

        [CanBeNull]
        string FaxNumber { get; set; }

        [CanBeNull]
        string FirstName { get; set; }

        [CanBeNull]
        string FullName { get; set; }

        [CanBeNull]
        string HomeDirectory { get; set; }

        [CanBeNull]
        string HomePage { get; set; }

        bool AccountLocked { get; }

        [CanBeNull]
        DateTime? LastFailedLogin { get; }

        [CanBeNull]
        DateTime? LastLogin { get; }

        [CanBeNull]
        DateTime? LastLogoff { get; }

        [CanBeNull]
        string LastName { get; set; }

        [CanBeNull]
        string LoginScript { get; set; }

        [CanBeNull]
        IUser Manager { get; set; }

        long? MaxStorage { get; set; }

        [CanBeNull]
        string NamePrefix { get; set; }

        [CanBeNull]
        string NameSuffix { get; set; }

        [CanBeNull]
        string OfficeLocation { get; set; }

        [CanBeNull]
        string OtherName { get; set; }

        [CanBeNull]
        DateTime? PasswordLastChanged { get; }

        [CanBeNull]
        bool PasswordRequired { get; set; }

        [CanBeNull]
        string PostalAddress { get; set; }

        [CanBeNull]
        string PostalCode { get; set; }

        [NotNull]
        string ProfilePath { get; set; }

        IMemberOfCollection MemberOf { get; }

        [NotNull]
        IDirectoryObjectCollection SeeAlso { get; }
    }
}