using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Moq;

namespace Bstm.UnitTesting
{
    public static class AutoFixtureExtensions
    {
        public static T FreezeMock<T>(this IFixture fixture, MockBehavior mockBehavior = MockBehavior.Default)
            where T : class
        {
            return fixture.FreezeMock<T>(GetCtorArgTypes<T>(), mockBehavior);
        }

        public static T FreezeMock<T>(
            this IFixture fixture,
            Type[] ctorArgTypes,
            MockBehavior mockBehavior = MockBehavior.Default)
            where T : class
        {
            return fixture.FreezeMock<T>(CreateCtorArgs(fixture, ctorArgTypes), mockBehavior);
        }

        public static T FreezeMock<T>(
            this IFixture fixture,
            object[] ctorArgs,
            MockBehavior mockBehavior = MockBehavior.Default)
            where T : class
        {
            var mock = new Mock<T>(mockBehavior, ctorArgs);
            fixture.Inject(mock.Object);

            return mock.Object;
        }

        public static T CreateMock<T>(this IFixture fixture)
            where T : class
        {
            return fixture.CreateMock<T>(GetCtorArgTypes<T>());
        }

        public static T CreateMock<T>(this IFixture fixture, Type[] ctorArgTypes)
            where T : class
        {
            return fixture.CreateMock<T>(CreateCtorArgs(fixture, ctorArgTypes));
        }

        public static T CreateMock<T>(this IFixture fixture, object[] ctorArgs)
            where T : class
        {
            var mock = new Mock<T>(ctorArgs);

            return mock.Object;
        }

        private static Type[] GetCtorArgTypes<T>()
            where T : class
        {
            var type = typeof(T);
            var ctors = type.GetConstructors();

            if (ctors.Length > 1)
                throw new InvalidOperationException(
                    $"Found more then one public constructor for type {type.Name}. Specify required constructor arguments types explicitly.");

            var ctor = ctors.SingleOrDefault();
            var ctorArgTypes = ctor?.GetParameters().Select(c => c.ParameterType).ToArray() ?? new Type[0];
            return ctorArgTypes;
        }

        private static object[] CreateCtorArgs(IFixture fixture, Type[] ctorArgumentTypes)
        {
            if (ctorArgumentTypes.Length == 0)
                return new object[0];

            var context = new SpecimenContext(fixture);

            return ctorArgumentTypes
                .Select(t => t.IsInterface ? CreateMockForType(t) : context.Resolve(t))
                .ToArray();
        }

        private static object CreateMockForType(Type targetType)
        {
            var mockOf = typeof(Mock).GetMethod("Of", new Type[0]);
            var mockOfGeneric = mockOf.MakeGenericMethod(targetType);
            var result = mockOfGeneric.Invoke(null, new object[0]);
            return result;
        }
    }
}