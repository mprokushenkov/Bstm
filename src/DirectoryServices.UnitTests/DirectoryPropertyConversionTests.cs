using System;
using System.Linq;
using ActiveDs;
using Bstm.Common;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DirectoryPropertyConversionTests : TestBase
    {
        public DirectoryPropertyConversionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void OctetStringPropertyShouldCreateCorrectSearchFilterString()
        {
            // Fixture setup
            var guid = Guid.Parse("{3764CBC6-C740-46E3-8291-2C1D7CA3CFA1}");

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid?));

            // Exercise system
            var results = properties.Select(p => p.CreateSearchFilterString(guid));

            // Verify outcome
            results.ForEach(r => r.Should().Be(@"\c6\cb\64\37\40\c7\e3\46\82\91\2c\1d\7c\a3\cf\a1"));
        }

        [Fact]
        public void GuidPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid));

            // Exercise system
            var guids = properties
                .Select(p => p.ConvertToDirectoryValue(guid))
                .OfType<byte[]>()
                .Select(ar => new Guid(ar))
                .ToList();

            // Verify outcome
            guids.Should().HaveCountGreaterThan(0);
            guids.ForEach(r => r.Should().Be(guid));
        }

        [Fact]
        public void GuidPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid));

            // Exercise system
            var guids = properties
                .Select(p => p.ConvertFromDirectoryValue(guid.ToByteArray()))
                .ToList();

            // Verify outcome
            guids.Should().HaveCountGreaterThan(0);
            guids.ForEach(r => r.Should().Be(guid));
        }

        [Fact]
        public void DNPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.DNString && p.NotionalType == typeof(DN));

            // Exercise system
            var dns = properties
                .Select(p => p.ConvertToDirectoryValue(dn))
                .OfType<string>()
                .Select(DN.Parse)
                .ToList();

            // Verify outcome
            dns.Should().HaveCountGreaterThan(0);
            dns.ForEach(r => r.Should().Be(dn));
        }

        [Fact]
        public void DNPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.DNString && p.NotionalType == typeof(DN));

            // Exercise system
            var dns = properties
                .Select(p => p.ConvertFromDirectoryValue("CN=John"))
                .ToList();

            // Verify outcome
            dns.Should().HaveCountGreaterThan(0);
            dns.ForEach(r => r.Should().Be(dn));
        }

        [Fact]
        public void AdsUserFlagPropertyShouldConvertToCorrectDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.Enumeration
                            && p.NotionalType.IsEquivalentTo(typeof(ADS_USER_FLAG)));

            // Exercise system
            var flags = properties
                .Select(p => p.ConvertToDirectoryValue(userFlag))
                .OfType<int>()
                .Select(i => (ADS_USER_FLAG) i)
                .ToList();

            // Verify outcome
            flags.Should().HaveCountGreaterThan(0);
            flags.ForEach(r => r.Should().Be(userFlag));
        }

        [Fact]
        public void AdsUserFlagPropertyShouldCorrectConvertFromDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Enumeration
                .GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.Enumeration
                            && p.NotionalType.IsEquivalentTo(typeof(ADS_USER_FLAG)));

            // Exercise system
            var flags = properties
                .Select(p => p.ConvertFromDirectoryValue((int) userFlag))
                .ToList();

            // Verify outcome
            flags.Should().HaveCountGreaterThan(0);
            flags.ForEach(f => ((ADS_USER_FLAG) f).Should().Be(userFlag));
        }

        [Fact]
        public void ConvertFromDirectoryValueReturnsNullOnNullInput()
        {
            // Fixture setup

            // Exercise system and verify outcome
            DirectoryProperty.DistinguishedName.ConvertFromDirectoryValue(null).Should().BeNull();
        }

        [Fact]
        public void ExceptionShouldBeThrownForAttemptConvertValueInappropriateSyntax()
        {
            // Fixture setup

            // Exercise system
            Action call = () => DirectoryProperty.DistinguishedName.ConvertFromDirectoryValue("non DN string");

            // Verify outcome
            call.Should().Throw<DirectoryServicesException>()
                .WithMessage("Value 'non DN string' of type 'System.String' not suitable to syntax 'DNString' of directory property 'distinguishedName'.");
        }

        [Fact]
        public void DateTimeOffsetFromLargeIntegerShouldReturnNullForVeryBigValue()
        {
            // Fixture setup
            var largeInteger = DirectoryProperty.Int64ToLargeInteger(DateTimeOffset.MaxValue.ToFileTime() + 1);

            // Exercise system
            var offset = DirectoryProperty.DateTimeOffsetFromLargeInteger(largeInteger);

            // Verify outcome
            offset.Should().BeNull();
        }

        [Fact]
        public void DateTimeOffsetFromLargeIntegerShouldReturnNullForSmallValue()
        {
            // Fixture setup
            var largeInteger = DirectoryProperty.Int64ToLargeInteger(-1);

            // Exercise system
            var offset = DirectoryProperty.DateTimeOffsetFromLargeInteger(largeInteger);

            // Verify outcome
            offset.Should().BeNull();
        }

        [Fact]
        public void DateTimeOffsetFromLargeIntegerShouldReturnCorrectOffset()
        {
            // Fixture setup
            var expected = DateTimeOffset.Now;
            var largeInteger = DirectoryProperty.DateTimeOffsetToLargeInteger(expected);

            // Exercise system
            var actual = DirectoryProperty.DateTimeOffsetFromLargeInteger(largeInteger);

            // Verify outcome
            actual.Should().Be(expected);
        }

        [Fact]
        public void DateTimeOffsetToLargeIntegerShouldReturnZeroForVerySmallOffset()
        {
            // Fixture setup

            // Exercise system
            var largeInteger = DirectoryProperty.DateTimeOffsetToLargeInteger(DateTimeOffset.MinValue);

            // Verify outcome
            largeInteger.HighPart.Should().Be(0);
            largeInteger.LowPart.Should().Be(0);
        }
    }
}