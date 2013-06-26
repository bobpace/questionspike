using System;
using System.Linq.Expressions;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    //TODO: move these to FubuCore, it currently has an overload that supports object but not an open generic for TProperty
    public static class AccessorExtensions
    {
        public static Accessor ToAccessor<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            return GetAccessor(expression);
        }

        public static Accessor GetAccessor<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body is MethodCallExpression)
            {
                return ReflectionHelper.GetAccessor(expression.Body);
            }
            return ReflectionHelper.GetAccessor(getMemberExpression(expression));
        }

        private static MemberExpression getMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
        {
            var memberExpression = (MemberExpression) null;
            if (expression.Body.NodeType == ExpressionType.Convert)
                memberExpression = ((UnaryExpression) expression.Body).Operand as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "member");
            }
            return memberExpression;
        }
    }
}