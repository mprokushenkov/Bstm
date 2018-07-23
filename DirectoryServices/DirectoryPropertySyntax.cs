using Bstm.Common;

namespace Bstm.DirectoryServices
{
    public class DirectoryPropertySyntax : Enumeration
    {
        private DirectoryPropertySyntax(string name, string notes) : base(name)
        {
            Notes = notes;
        }

        // ReSharper disable once InconsistentNaming
        public static DirectoryPropertySyntax DNString { get; } =
            new DirectoryPropertySyntax("DNString", "Standard distinguished name (DN) syntax");

        public static DirectoryPropertySyntax ObjectIdentifierString { get; } =
            new DirectoryPropertySyntax("ObjectIdentifierString", "Contains only digits and \".\"");

        public static DirectoryPropertySyntax TeletextString { get; } =
            new DirectoryPropertySyntax("TeletextString", "Case insensitive for searching; Teletex characters only");

        public static DirectoryPropertySyntax PrintableString { get; } =
            new DirectoryPropertySyntax("PrintableString", "Case sensitive for searching; printable characters only");

        // ReSharper disable once InconsistentNaming
        public static DirectoryPropertySyntax IA5String { get; } =
            new DirectoryPropertySyntax("IA5String", "Case sensitive for searching; IA5 string");

        public static DirectoryPropertySyntax NumericString { get; } =
            new DirectoryPropertySyntax("NumericString", "Contains only digits; rarely used in Active Directory");

        public static DirectoryPropertySyntax ObjectDNBinary { get; } =
            new DirectoryPropertySyntax("ObjectDNBinary", "Also Object(OR-Name); used for associating a GUID with DN");

        public static DirectoryPropertySyntax Boolean { get; } =
            new DirectoryPropertySyntax("Boolean", "Used for standard Boolean values");

        public static DirectoryPropertySyntax Integer { get; } =
            new DirectoryPropertySyntax("Integer", "Used for standard signed integers");

        public static DirectoryPropertySyntax Enumeration { get; } =
            new DirectoryPropertySyntax("Enumeration", "Used for enumerated values");

        public static DirectoryPropertySyntax OctetString { get; } =
            new DirectoryPropertySyntax("OctetString", "Used for arbitrary binary data");

        public static DirectoryPropertySyntax ObjectReplicaLink { get; } =
            new DirectoryPropertySyntax("ObjectReplicaLink", "Used by the system only for replication");

        public static DirectoryPropertySyntax UtcTimeString { get; } =
            new DirectoryPropertySyntax("UtcTimeString", "Used for date values; stored relative to UTC");

        public static DirectoryPropertySyntax GeneralizedTimeString { get; } =
            new DirectoryPropertySyntax("GeneralizedTimeString", "Used for date values; time zone information is included");

        public static DirectoryPropertySyntax UnicodeString { get; } =
            new DirectoryPropertySyntax("UnicodeString", "Case insensitive for searching; contains any Unicode character");

        public static DirectoryPropertySyntax ObjectPresentationAddress { get; } =
            new DirectoryPropertySyntax("ObjectPresentationAddress", "Not really used in Active Directory either");

        public static DirectoryPropertySyntax ObjectDNString { get; } =
            new DirectoryPropertySyntax("ObjectDNString", "Not used in Active Directory schema; also defined as Object (Access-Point) which is not used and has no marshaling defined");

        // ReSharper disable once InconsistentNaming
        public static DirectoryPropertySyntax NTSecurityDescriptorString { get; } =
            new DirectoryPropertySyntax("NTSecurityDescriptorString", "Contains Windows security descriptors");

        public static DirectoryPropertySyntax LargeInteger { get; } =
            new DirectoryPropertySyntax("LargeInteger", "Represents a 64-bit signed integer value");

        public static DirectoryPropertySyntax Interval { get; } =
            new DirectoryPropertySyntax("Interval", "Same as LargeInteger but Interval is treated as unsigned");

        public static DirectoryPropertySyntax SidString { get; } =
            new DirectoryPropertySyntax("SidString", "Contains Windows security identifiers");

        public string Notes { get; }
    }
}