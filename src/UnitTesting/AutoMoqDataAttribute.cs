using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Bstm.UnitTesting
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : this(CreateFixture)
        {
        }

        protected AutoMoqDataAttribute(Func<IFixture> fixtureFactory) : base(fixtureFactory)
        {
        }

        protected static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}