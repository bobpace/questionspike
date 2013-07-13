using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EligibilityQuestions
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
}