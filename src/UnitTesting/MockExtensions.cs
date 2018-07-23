using System;
using System.Linq.Expressions;
using Moq;
using Moq.Language;
using Moq.Language.Flow;

namespace Bstm.UnitTesting
{
    public static class MockExtensions
    {
        public static ISetup<T> Setup<T>(this T subject, Expression<Action<T>> expression)
            where T : class
        {
            return Mock.Get(subject).Setup(expression);
        }

        public static ISetup<T, TResult> Setup<T, TResult>(this T subject,  Expression<Func<T, TResult>> expression)
            where T : class
        {
            return Mock.Get(subject).Setup(expression);
        }

        public static ISetupSequentialAction SetupSequence<T>(this T subject, Expression<Action<T>> expression)
            where T : class
        {
            return Mock.Get(subject).SetupSequence(expression);
        }

        public static ISetupSequentialResult<TResult> SetupSequence<T, TResult>(this T subject,  Expression<Func<T, TResult>> expression)
            where T : class
        {
            return Mock.Get(subject).SetupSequence(expression);
        }

        public static void Verify<T>(this T subject, Expression<Action<T>> expression)
            where T : class
        {
            Mock.Get(subject).Verify(expression);
        }

        public static void VerifySet<T>(this T subject, Action<T> setterAction)
            where T : class
        {
            Mock.Get(subject).VerifySet(setterAction);
        }

        public static void Verify<T>(this T subject, Expression<Action<T>> expression, Times times)
            where T : class
        {
            Mock.Get(subject).Verify(expression, times);
        }

        public static void VerifyAll<T>(this T subject)
            where T : class
        {
            Mock.Get(subject).VerifyAll();
        }

        public static ISetup<T> SetupIgnoreArgs<T>(this T subject, Expression<Action<T>> expression)
            where T : class
        {
            expression = new MakeAnyVisitor().VisitAndConvert(expression, "SetupIgnoreArgs");
            return Mock.Get(subject).Setup(expression);
        }

        public static ISetup<T, TResult> SetupIgnoreArgs<T, TResult>(
            this T subject,
            Expression<Func<T, TResult>> expression)
            where T : class
        {
            expression = new MakeAnyVisitor().VisitAndConvert(expression, "SetupIgnoreArgs");
            return Mock.Get(subject).Setup(expression);
        }

        private class MakeAnyVisitor : ExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (node.Value != null)
                    return base.VisitConstant(node);

                var method = typeof(It)
                    .GetMethod("IsAny")
                    .MakeGenericMethod(node.Type);

                return Expression.Call(method);
            }
        }
    }
}