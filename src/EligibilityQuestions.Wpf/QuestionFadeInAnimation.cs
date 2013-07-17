using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace EligibilityQuestions.Wpf
{
    public class QuestionFadeInAnimation : DoubleAnimation
    {
        public object Question
        {
            get { return GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register("Question", typeof(object), typeof(QuestionFadeInAnimation), new PropertyMetadata(null));


        public QuestionFadeInAnimation()
        {
            To = 1;
            From = 0;
            Duration = new Duration(TimeSpan.FromMilliseconds(500));
            Storyboard.SetTargetProperty(this, new PropertyPath("Opacity"));
        }

        protected override Freezable CreateInstanceCore()
        {
            //Only run the animation on secondary questions.
            //The binding engine tries to convert a DisconnectedObject to Question
            //if the property is strongly typed and throws errors which makes the UI lag for a second.
            var question = Question as Question;
            if (question != null && question.Rank == QuestionRank.Primary)
                From = 1;

            return base.CreateInstanceCore();
        }
    }
}