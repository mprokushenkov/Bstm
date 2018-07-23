using System;
using System.Linq;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using EqualityTests;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.Common.UnitTests
{   
    public class EnumerationTests : TestBase
    {
        public EnumerationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Theory]
        [AutoMoqData]
        public void CtorArgsCopiedToAppropriateProperties(ConstructorInitializedMemberAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(Enumeration).GetProperties());
        }

        [Fact]
        public void EqualityMembersCorrectDefined()
        {
            // Fixture setup
            EqualityTestsFor<Enumeration>.Assert();
        }

        [Fact]
        public void GetAllReturnsAllStaticProperties()
        {
            // Fixture setup
            var all = Enumeration.GetAll<Color>()?.ToList();

            // Exercise system and verify outcome
            all.Should().NotBeNull();
            all.Should().HaveCount(2);

            // ReSharper disable PossibleNullReferenceException
            all[0].Should().Be(Color.Red); //-V3105
            all[1].Should().Be(Color.Green); //-V3105
        }

        [Fact]
        public void FromNameReturnsDesiredInstance()
        {
            // Fixture setup
            var fromName = Enumeration.FromName<Color>(Color.Red.Name);

            // Exercise system and verify outcome
            fromName.Should().Be(Color.Red);
        }

        [Fact]
        public void FromNameThrowsExceptionForInvalidValue()
        {
            // Fixture setup
            Action call = () => Enumeration.FromName<Color>("Hello");

            // Exercise system and verify outcome
            call.Should().Throw<ArgumentOutOfRangeException>();
        }

        private class Color : Enumeration
        {
            private Color(string name) : base(name)
            {
            }

            public static Color Red { get; } = new Color("Red");

            public static Color Green { get; } = new Color("Green");
        }
    }
}