using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DirectoryPropertySyntaxTests : TestBase
    {
        public DirectoryPropertySyntaxTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void SyntaxNamesCorrectDefined()
        {
            // Fixture setup

            // Exercise system and verify outcome
            DirectoryPropertySyntax.DNString.Name.Should().Be("DNString");
            DirectoryPropertySyntax.ObjectIdentifierString.Name.Should().Be("ObjectIdentifierString");
            DirectoryPropertySyntax.TeletextString.Name.Should().Be("TeletextString");
            DirectoryPropertySyntax.PrintableString.Name.Should().Be("PrintableString");
            DirectoryPropertySyntax.IA5String.Name.Should().Be("IA5String");
            DirectoryPropertySyntax.NumericString.Name.Should().Be("NumericString");
            DirectoryPropertySyntax.ObjectDNBinary.Name.Should().Be("ObjectDNBinary");
            DirectoryPropertySyntax.Boolean.Name.Should().Be("Boolean");
            DirectoryPropertySyntax.Integer.Name.Should().Be("Integer");
            DirectoryPropertySyntax.Enumeration.Name.Should().Be("Enumeration");
            DirectoryPropertySyntax.OctetString.Name.Should().Be("OctetString");
            DirectoryPropertySyntax.ObjectReplicaLink.Name.Should().Be("ObjectReplicaLink");
            DirectoryPropertySyntax.UtcTimeString.Name.Should().Be("UtcTimeString");
            DirectoryPropertySyntax.GeneralizedTimeString.Name.Should().Be("GeneralizedTimeString");
            DirectoryPropertySyntax.UnicodeString.Name.Should().Be("UnicodeString");
            DirectoryPropertySyntax.ObjectPresentationAddress.Name.Should().Be("ObjectPresentationAddress");
            DirectoryPropertySyntax.ObjectDNString.Name.Should().Be("ObjectDNString");
            DirectoryPropertySyntax.NTSecurityDescriptorString.Name.Should().Be("NTSecurityDescriptorString");
            DirectoryPropertySyntax.LargeInteger.Name.Should().Be("LargeInteger");
            DirectoryPropertySyntax.Interval.Name.Should().Be("Interval");
            DirectoryPropertySyntax.SidString.Name.Should().Be("SidString");
        }
    }
}