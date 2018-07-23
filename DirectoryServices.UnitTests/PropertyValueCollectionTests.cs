using System.Linq;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public sealed class PropertyValueCollectionTests : TestBase
    {
        public PropertyValueCollectionTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup
            var ignore = new[] {"SetValue", "CopyTo", "LoadFrom", "RemoveValue", "AppendValue"};

            // Exercise system and verify outcome
            assertion.Verify(typeof(PropertyValueCollection).GetMethods().Where(m => !ignore.Contains(m.Name)));
        }

        [Theory]
        [AutoMoqData]
        public void OnePropertyValueShouldBeSet(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValue(DirectoryProperty.Member, "CN=John");

            // Verify outcome
            collection[DirectoryProperty.Member].Should().BeEquivalentTo(DN.Parse("CN=John"));
        }

        [Theory]
        [AutoMoqData]
        public void ManyPropertyValuesShouldBeSet(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {"CN=John", "CN=Paul"});

            // Verify outcome
            collection[DirectoryProperty.Member].Should().BeEquivalentTo(DN.Parse("CN=John"), DN.Parse("CN=Paul"));
        }

        [Theory]
        [AutoMoqData]
        public void CollectionShouldBeCleared(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValue(DirectoryProperty.Member, "John");
            collection.Clear();

            // Verify outcome
            collection[DirectoryProperty.Member].Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void OneTypedPropertyValueShouldBeReaded(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValue(DirectoryProperty.Member, "CN=John");

            // Verify outcome
            collection.GetValue<DN>(DirectoryProperty.Member).Should().Be(DN.Parse("CN=John"));
        }

        [Theory]
        [AutoMoqData]
        public void ManyTypedPropertyValuesShouldBeReaded(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {"CN=John", "CN=Paul"});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(DN.Parse("CN=John"), DN.Parse("CN=Paul"));
        }

        [Theory]
        [AutoMoqData]
        public void ExistingPropertyValuesShouldBeRewritten(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValues(DirectoryProperty.Member, new object[] {"CN=John", "CN=Paul"});

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {"CN=John", "CN=Paul", "CN=Ringo"});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(DN.Parse("CN=John"), DN.Parse("CN=Paul"), DN.Parse("CN=Ringo"));
        }

        [Theory]
        [AutoMoqData]
        public void OnePropertyValueShouldBeRemoved(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, "CN=John");

            // Exercise system
            collection.RemoveValue(DirectoryProperty.Member, "CN=John");

            // Verify outcome
            collection[DirectoryProperty.Member].Should().HaveCount(0);
        }

        [Theory]
        [AutoMoqData]
        public void AllPropertyValueShouldBeRemoved(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, new object[] {"CN=John", "CN=Paul"});

            // Exercise system
            collection.RemoveValues(DirectoryProperty.Member);

            // Verify outcome
            collection[DirectoryProperty.Member].Should().HaveCount(0);
        }

        [Theory]
        [AutoMoqData]
        public void OnePropertyValueShouldBeAppended(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, "CN=John");

            // Exercise system
            collection.AppendValue(DirectoryProperty.Member, "CN=Paul");

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(DN.Parse("CN=John"), DN.Parse("CN=Paul"));
        }

        [Theory]
        [AutoMoqData]
        public void ManyPropertyValuesShouldBeAppended(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, "CN=John");

            // Exercise system
            collection.AppendValues(DirectoryProperty.Member, new object[] {"CN=Paul", "CN=Ringo"});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(DN.Parse("CN=John"), DN.Parse("CN=Paul"), DN.Parse("CN=Ringo"));
        }
    }
}