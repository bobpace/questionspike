using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public static class ExtensionMethods
    {
        public static Type UnwrapNullable(this Type type)
        {
            return type.IsNullable() ? type.GetGenericArguments().First() : type;
        }

        public static void ValidateFlagsEnumType(this Type type)
        {
            if (!type.IsEnum || !type.HasAttribute<FlagsAttribute>())
            {
                throw new ArgumentException("Use a flags enum for TFlagsEnum");
            }
        }

        public static string GetPropertyName(this LambdaExpression property)
        {
            var memberExpression = (MemberExpression) property.Body;
            return memberExpression.Member.Name;
        }

        public static bool HasFlag(this int value, int otherValue)
        {
            if (value == 0 && otherValue == 0) return true;
            return (value & otherValue) != 0;
        }

        public static string ProperetyValuesToString(this object instance)
        {
            return instance.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => "{0}={1}".ToFormat(x.Name, x.GetValue(instance, null) ?? "null"))
                .Join(Environment.NewLine);
        }
    }

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