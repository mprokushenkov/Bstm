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
        private readonly DN john = DN.Parse("CN=John");
        private readonly DN paul = DN.Parse("CN=John");
        private readonly DN ringo = DN.Parse("CN=John");

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
            collection.SetValue(DirectoryProperty.Member, john);

            // Verify outcome
            collection[DirectoryProperty.Member].Should().BeEquivalentTo(paul);
        }

        [Theory]
        [AutoMoqData]
        public void ManyPropertyValuesShouldBeSet(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {john, paul });

            // Verify outcome
            collection[DirectoryProperty.Member].Should().BeEquivalentTo(john, paul);
        }

        [Theory]
        [AutoMoqData]
        public void CollectionShouldBeCleared(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValue(DirectoryProperty.Member, john);
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
            collection.SetValue(DirectoryProperty.Member, john);

            // Verify outcome
            collection.GetValue<DN>(DirectoryProperty.Member).Should().Be(DN.Parse(john));
        }

        [Theory]
        [AutoMoqData]
        public void ManyTypedPropertyValuesShouldBeReaded(PropertyValueCollection collection)
        {
            // Fixture setup

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {john, paul});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(john, paul);
        }

        [Theory]
        [AutoMoqData]
        public void ExistingPropertyValuesShouldBeRewritten(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValues(DirectoryProperty.Member, new object[] {john, paul});

            // Exercise system
            collection.SetValues(DirectoryProperty.Member, new object[] {john, paul, ringo});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(john, paul, ringo);
        }

        [Theory]
        [AutoMoqData]
        public void OnePropertyValueShouldBeRemoved(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, john);

            // Exercise system
            collection.RemoveValue(DirectoryProperty.Member, paul);

            // Verify outcome
            collection[DirectoryProperty.Member].Should().HaveCount(0);
        }

        [Theory]
        [AutoMoqData]
        public void AllPropertyValueShouldBeRemoved(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, new object[] {john, paul});

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
            collection.SetValue(DirectoryProperty.Member, DN.Parse(john));

            // Exercise system
            collection.AppendValue(DirectoryProperty.Member, DN.Parse(paul));

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(DN.Parse(john), DN.Parse(paul));
        }

        [Theory]
        [AutoMoqData]
        public void ManyPropertyValuesShouldBeAppended(PropertyValueCollection collection)
        {
            // Fixture setup
            collection.SetValue(DirectoryProperty.Member, john);

            // Exercise system
            collection.AppendValues(DirectoryProperty.Member, new object[] {paul, ringo});

            // Verify outcome
            collection.GetValues<DN>(DirectoryProperty.Member)
                .Should().BeEquivalentTo(john, paul, ringo);
        }
    }
}