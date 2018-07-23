using KellermanSoftware.CompareNetObjects;
using Moq;

namespace Bstm.UnitTesting
{
    public static class TestingExtensions
    {
        public static T AsLikeness<T>(this T subject) where T : class
        {
            return It.Is<T>(s => Compare(s, subject));
        }

        private static bool Compare(object a, object b)
        {
            var compareResult = new CompareLogic().Compare(a, b);
            return compareResult.AreEqual;
        }
    }
}