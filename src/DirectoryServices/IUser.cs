using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public interface IUser : IDirectoryObject
    {
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

        IMemberOfCollection MemberOf { get; }
    }
}