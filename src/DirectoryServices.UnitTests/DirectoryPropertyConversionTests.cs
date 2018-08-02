using System;
using System.Linq;
using ActiveDs;
using Bstm.Common;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Bstm.DirectoryServices.DirectoryProperty;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class DirectoryPropertyConversionTests : TestBase
    {
        const long filetime = 131771579408071845;

        public DirectoryPropertyConversionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void OctetStringPropertyShouldCreateSearchFilterString()
        {
            // Fixture setup
            var guid = Guid.Parse("{3764CBC6-C740-46E3-8291-2C1D7CA3CFA1}");

            var properties = Enumeration.GetAll<DirectoryProperty>()
                .Where(p => p.Syntax == DirectoryPropertySyntax.OctetString && p.NotionalType == typeof(Guid?));

            // Exercise system
            var results = properties.Select(p => p.CreateSearchFilterString(guid));

            // Verify outcome
            results.ForEach(r => r.Should().Be(@"\c6\cb\64\37\40\c7\e3\46\82\91\2c\1d\7c\a3\cf\a1"));
        }

        [Fact]
        public void GuidPropertyShouldConvertToDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
        public void GuidPropertyShouldConvertFromDirectoryValue()
        {
            // Fixture setup
            var guid = Guid.NewGuid();

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
        public void DNPropertyShouldConvertToDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
        public void DNPropertyShouldConvertFromDirectoryValue()
        {
            // Fixture setup
            var dn = DN.Parse("CN=John");

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
        public void AdsUserFlagPropertyShouldConvertToDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
        public void AdsUserFlagPropertyShouldConvertFromDirectoryValue()
        {
            // Fixture setup
            const ADS_USER_FLAG userFlag = ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE;

            var properties = Enumeration.GetAll<DirectoryProperty>()
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
            DistinguishedName.ConvertFromDirectoryValue(null).Should().BeNull();
        }

        [Fact]
        public void ExceptionShouldBeThrownForAttemptConvertValueInappropriateSyntax()
        {
            // Fixture setup

            // Exercise system
            Action call = () => DistinguishedName.ConvertFromDirectoryValue("non DN string");

            // Verify outcome
            call.Should().Throw<DirectoryServicesException>()
                .WithMessage("Value 'non DN string' of type 'System.String' not suitable to syntax 'DNString' of directory property 'distinguishedName'.");
        }

        [Fact]
        public void DateTimeFromLargeIntegerShouldReturnNullForSmallValue()
        {
            // Fixture setup
            var largeInteger = Int64ToLargeInteger(-1);

            // Exercise system
            var dateTime = DateTimeFromLargeInteger(largeInteger);

            // Verify outcome
            dateTime.Should().BeNull();
        }

        [Fact]
        public void DateTimeFromLargeIntegerShouldReturnDateTime()
        {
            // Fixture setup
            var expected = DateTime.FromFileTimeUtc(filetime);
            var largeInteger = DateTimeToLargeInteger(expected);

            // Exercise system
            var actual = DateTimeFromLargeInteger(largeInteger);

            // Verify outcome
            actual.Should().Be(expected);
        }

        [Fact]
        public void DateTimeFromLargeIntegerShouldReturnNullIfIntegerNoSet()
        {
            // Fixture setup
            var integer = new LargeInteger
            {
                HighPart = int.MaxValue,
                LowPart = -1
            };

            // Exercise system
            var actual = DateTimeFromLargeInteger(integer);

            // Verify outcome
            actual.Should().BeNull();
        }

        [Fact]
        public void DateTimeToLargeIntegerShouldReturnZeroForVerySmallDateTime()
        {
            // Fixture setup

            // Exercise system
            var largeInteger = DateTimeToLargeInteger(DateTime.MinValue);

            // Verify outcome
            largeInteger.HighPart.Should().Be(0);
            largeInteger.LowPart.Should().Be(0);
        }

        [Fact]
        public void DateTimePropertyShouldConvertToDirectoryValue()
        {
            // Fixture setup
            var dateTime = DateTime.FromFileTimeUtc(filetime);

            var properties = Enumeration.GetAll<DirectoryProperty>()
                .Where(p => p.NotionalType == typeof(DateTime?)
                            && p.DirectoryType.IsEquivalentTo(typeof(IADsLargeInteger)));

            // Exercise system
            var dateTimes = properties
                .Select(p => p.ConvertToDirectoryValue(dateTime))
                .OfType<IADsLargeInteger>()
                .Select(DateTimeFromLargeInteger)
                .ToList();

            // Verify outcome
            dateTimes.Should().HaveCountGreaterThan(0);
            dateTimes.ForEach(r => r.Should().Be(dateTime));
        }

        [Fact]
        public void DateTimePropertyShouldConvertFromDirectoryValue()
        {
            // Fixture setup
            var dateTime = DateTime.FromFileTimeUtc(filetime);

            var properties = Enumeration.GetAll<DirectoryProperty>()
                .Where(p => p.NotionalType == typeof(DateTime?)
                            && p.DirectoryType.IsEquivalentTo(typeof(IADsLargeInteger)));

            // Exercise system
            var dateTimes = properties
                .Select(p => p.ConvertFromDirectoryValue(DateTimeToLargeInteger(dateTime)))
                .ToList();

            // Verify outcome
            dateTimes.Should().HaveCountGreaterThan(0);
            dateTimes.ForEach(f => f.Should().Be(dateTime));
        }

        [Fact]
        public void LongLargeIntegerPropertyShouldConvertToDirectoryValue()
        {
            // Fixture setup
            var number = DateTime.Now.Ticks;

            var properties = Enumeration.GetAll<DirectoryProperty>()
                .Where(p => p.NotionalType == typeof(long)
                            && p.DirectoryType.IsEquivalentTo(typeof(IADsLargeInteger)));

            // Exercise system
            var numbers = properties
                .Select(p => p.ConvertToDirectoryValue(number))
                .OfType<IADsLargeInteger>()
                .Select(Int64FromLargeInteger)
                .ToList();

            // Verify outcome
            numbers.Should().HaveCountGreaterThan(0);
            numbers.ForEach(n => n.Should().Be(number));
        }

        [Fact]
        public void LongLargeIntegerPropertyShouldConvertFromDirectoryValue()
        {
            // Fixture setup
            var number = DateTime.Now.Ticks;

            var properties = Enumeration.GetAll<DirectoryProperty>()
                .Where(p => p.NotionalType == typeof(long)
                            && p.DirectoryType.IsEquivalentTo(typeof(IADsLargeInteger)));

            // Exercise system
            var numbers = properties
                .Select(p => p.ConvertFromDirectoryValue(Int64ToLargeInteger(number)))
                .ToList();

            // Verify outcome
            numbers.Should().HaveCountGreaterThan(0);
            numbers.ForEach(n => n.Should().Be(number));
        }
    }
}