using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using Xunit.Abstractions;

namespace Bstm.UnitTesting
{
    public class TestBase
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly Lazy<IFixture> fixtureLazy = new Lazy<IFixture>(CreateFixture);

        protected TestBase(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        protected IFixture Fixture => fixtureLazy.Value;

        protected void WriteLine(string format, params object[] args)
        {
            testOutputHelper.WriteLine(format, args);
        }

        protected void WriteLine(object o)
        {
            if (o == null)
            {
                return;
            }

            testOutputHelper.WriteLine(o.ToString());
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }

}