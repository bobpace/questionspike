using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ExtendHealth.Ssc.Framework
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.GetPropertyName()));
            }
        }
    }

    public static class ExtensionMethods
    {
        public static string GetPropertyName(this LambdaExpression property)
        {
            var memberExpression = (MemberExpression) property.Body;
            return memberExpression.Member.Name;
        }
    }
}