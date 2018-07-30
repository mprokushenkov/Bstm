using System;
using ActiveDs;
using Bstm.Common;

namespace Bstm.DirectoryServices
{
    public partial class DirectoryProperty : Enumeration
    {
        protected DirectoryProperty(
            string name,
            DirectoryPropertySyntax syntax,
            bool multivalued,
            Type notionalType,
            Type directoryType) : base(name)
        {
            Syntax = syntax;
            Multivalued = multivalued;
            NotionalType = notionalType;
            DirectoryType = directoryType;
        }

        public static DirectoryProperty Member { get; } =
            new DirectoryProperty("member", DirectoryPropertySyntax.DNString, true, typeof(DN), typeof(string));

        public static DirectoryProperty MemberOf { get; } =
            new DirectoryProperty("memberOf", DirectoryPropertySyntax.DNString, true, typeof(DN), typeof(string));

        public static DirectoryProperty ObjectClass { get; } =
            new DirectoryProperty("objectClass", DirectoryPropertySyntax.ObjectIdentifierString, true, typeof(string), typeof(string));

        public static DirectoryProperty DistinguishedName { get; } =
            new DirectoryProperty("distinguishedName", DirectoryPropertySyntax.DNString, false, typeof(DN), typeof(string));

        public static DirectoryProperty SamAccountName { get; } =
            new DirectoryProperty("samAccountName", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty DisplayName { get; } =
            new DirectoryProperty("displayName", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty GroupType { get; } =
            new DirectoryProperty("groupType", DirectoryPropertySyntax.Enumeration, false, typeof(string), typeof(int));

        public static DirectoryProperty ObjectGuid { get; } =
            new DirectoryProperty("objectGUID", DirectoryPropertySyntax.OctetString, false, typeof(Guid), typeof(byte[]));

        public static DirectoryProperty Department { get; } =
            new DirectoryProperty("department", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty Description { get; } =
            new DirectoryProperty("description", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty Division { get; } =
            new DirectoryProperty("division", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty Mail { get; } =
            new DirectoryProperty("mail", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty EmployeeId { get; } =
            new DirectoryProperty("employeeId", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty FacsimileTelephoneNumber { get; } =
            new DirectoryProperty("facsimileTelephoneNumber", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty GivenName { get; } =
            new DirectoryProperty("givenName", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty HomeDirectory { get; } =
            new DirectoryProperty("homeDirectory", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty WwwHomePage { get; } =
            new DirectoryProperty("wWWHomePage", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty UserAccountControl { get; } =
            new DirectoryProperty("userAccountControl", DirectoryPropertySyntax.Enumeration, false, typeof(ADS_USER_FLAG), typeof(int));

        public static DirectoryProperty AccountExpires { get; } =
            new DirectoryProperty("accountExpires", DirectoryPropertySyntax.Interval, false, typeof(DateTime?), typeof(IADsLargeInteger));

        public static DirectoryProperty BadPwdCount { get; } =
            new DirectoryProperty("badPwdCount", DirectoryPropertySyntax.Enumeration, false, typeof(int), typeof(int));

        public static DirectoryProperty LockoutTime { get; } = new DirectoryProperty(
            "lockoutTime",DirectoryPropertySyntax.Interval, false, typeof(long), typeof(IADsLargeInteger));

        public static DirectoryProperty LastLogon { get; } = new DirectoryProperty(
            "lastLogon", DirectoryPropertySyntax.Interval, false, typeof(DateTime?), typeof(IADsLargeInteger));

        public static DirectoryProperty BadPasswordTime { get; } = new DirectoryProperty(
            "badPasswordTime", DirectoryPropertySyntax.Interval, false, typeof(DateTime?), typeof(IADsLargeInteger));

        public static DirectoryProperty LastLogoff { get; } = new DirectoryProperty(
            "lastLogoff", DirectoryPropertySyntax.Interval, false, typeof(DateTime?), typeof(IADsLargeInteger));

        public static DirectoryProperty Sn { get; } = new DirectoryProperty(
            "sn", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty ScriptPath { get; } = new DirectoryProperty(
            "scriptPath", DirectoryPropertySyntax.UnicodeString, false, typeof(string), typeof(string));

        public static DirectoryProperty Manager { get; } = new DirectoryProperty(
            "manager", DirectoryPropertySyntax.DNString, false, typeof(DN), typeof(string));

        public DirectoryPropertySyntax Syntax { get; }

        public bool Multivalued { get; }

        public Type NotionalType { get; }

        public Type DirectoryType { get; }
    }
}