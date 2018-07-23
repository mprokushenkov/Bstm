using System.Reflection;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class SearchFilterTests : TestBase
    {
        public SearchFilterTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(SearchFilter).GetMethods(BindingFlags.Static));
        }

        [Fact]
        public void EqualityCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.Equality("displayName", "John Doe");

            // Verify outcome
            filter.ToString().Should().Be("(displayName=John Doe)");
        }

        [Fact]
        public void NegationCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.Negation("displayName", "John Doe");

            // Verify outcome
            filter.ToString().Should().Be("(!(displayName=John Doe))");
        }

        [Fact]
        public void PresenceCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.Presence("displayName");

            // Verify outcome
            filter.ToString().Should().Be("(displayName=*)");
        }

        [Fact]
        public void AbsenceCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.Absence("displayName");

            // Verify outcome
            filter.ToString().Should().Be("(!(displayName=*))");
        }

        [Fact]
        public void GreaterThanOrEqualCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.GreaterThanOrEqual("displayName", "John Doe");

            // Verify outcome
            filter.ToString().Should().Be("(displayName>=John Doe)");
        }

        [Fact]
        public void LessThanOrEqualCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.LessThanOrEqual("displayName", "John Doe");

            // Verify outcome
            filter.ToString().Should().Be("(displayName<=John Doe)");
        }

        [Fact]
        public void AndCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter
                .Equality("displayName", "John")
                .And(SearchFilter.Presence("mail"))
                .And(SearchFilter.Negation("sn", "Doe"));

            // Verify outcome
            filter.ToString().Should().Be("(&(displayName=John)(mail=*)(!(sn=Doe)))");
        }

        [Fact]
        public void OrCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter
                .Equality("displayName", "John")
                .Or(SearchFilter.Presence("mail"))
                .Or(SearchFilter.Negation("sn", "Doe"));

            // Verify outcome
            filter.ToString().Should().Be("(|(displayName=John)(mail=*)(!(sn=Doe)))");
        }

        [Fact]
        public void AndWithOrCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter
                .Equality("displayName", "John")
                .And(SearchFilter.Presence("mail"))
                .Or(SearchFilter.Equality("sn", "Doe"));

            // Verify outcome
            filter.ToString().Should().Be("(|(&(displayName=John)(mail=*))(sn=Doe))");
        }

        [Fact]
        public void OrWithAndCreatesCorrectSearchFilter()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter
                .Equality("displayName", "John")
                .Or(SearchFilter.Presence("mail"))
                .And(SearchFilter.Equality("sn", "Doe"));

            // Verify outcome
            filter.ToString().Should().Be("(&(|(displayName=John)(mail=*))(sn=Doe))");
        }

        [Fact]
        public void ValueReturnsCorrectSearchFilterRepresentation()
        {
            // Fixture setup

            // Exercise system
            var filter = SearchFilter.GreaterThanOrEqual("displayName", "John Doe");

            // Verify outcome
            filter.Value.Should().Be("(displayName>=John Doe)");
        }

        [Fact]
        public void ImplicitConversionOperatorReturnsCorrectSearchFilterRepresentation()
        {
            // Fixture setup

            // Exercise system
            string filter = SearchFilter.GreaterThanOrEqual("displayName", "John Doe");

            // Verify outcome
            filter.Should().Be("(displayName>=John Doe)");
        }
    }
}