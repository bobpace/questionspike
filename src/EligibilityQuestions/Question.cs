﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuCore.Reflection;

namespace EligibilityQuestions
{
    public delegate Question NextQuestion(Question question);

    public enum QuestionRank
    {
        Secondary,
        Primary
    }

    public abstract class Question : NotifyPropertyChangedBase
    {
        //convenience for default end of question chain
        protected static NextQuestion Done = (x => null);

        private object _answer;

        public Accessor Accessor { get; set; }
        public string QuestionText { get; set; }
        public string HelpText { get; set; }
        public QuestionRank Rank { get; set; }

        public virtual object Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                _answerUpdated = true;
                NotifyOfPropertyChange(() => Answer);
                NotifyOfPropertyChange(() => NextQuestion);
            }
        }

        public abstract NextQuestion GetNextQuestion();

        private bool _answerUpdated;
        private Question _nextQuestion;
        public Question NextQuestion
        {
            get
            {
                if (Answer == null) return null;
                if (_answerUpdated)
                {
                    _nextQuestion = GetNextQuestion()(this);
                    _answerUpdated = false;
                }
                return _nextQuestion;
            }
        }

        public void SetAnswerFromModel(object model)
        {
            Answer = Accessor.GetValue(model);
        }

        /// <summary>
        /// Extra questions that relate to this question
        /// Can change depending on the Answer if you need it to
        /// They will be considered before NextQuestion in the question chain
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<Question> ExtraQuestions()
        {
            yield break;
        }

        public virtual IEnumerable<Question> AnsweredQuestions()
        {
            //this question, followed by any extra answered questions, followed by the next question in the chains answered questions
            var result = new[] {this}.Concat(ExtraQuestions());
            if (NextQuestion != null)
            {
                result = result.Concat(NextQuestion.AnsweredQuestions());
            }
            return result;
        }

        //////////////////
        //Factory methods/
        //////////////////
        public static YesNoQuestion ForAnswer<TResult>(Expression<Func<TResult, bool?>> accessor)
        {
            return ForAnswer<TResult, YesNoQuestion, bool?>(accessor);
        }

        public static DateTimeQuestion ForAnswer<TResult>(Expression<Func<TResult, DateTime?>> accessor)
        {
            return ForAnswer<TResult, DateTimeQuestion, DateTime?>(accessor);
        }

        public static MultipleSelectQuestion ForAnswer<TResult, TFlagsEnum>(Expression<Func<TResult, TFlagsEnum>> accessor)
        {
            var typeToCheck = typeof (TFlagsEnum).UnwrapNullable();
            typeToCheck.ValidateFlagsEnumType();
            return ForAnswer<TResult, MultipleSelectQuestion, TFlagsEnum>(accessor);
        }

        private static TQuestion ForAnswer<TResult, TQuestion, TProperty>(Expression<Func<TResult, TProperty>> accessor)
            where TQuestion : Question, new()
        {
            var question = new TQuestion
            {
                Accessor = accessor.ToAccessor()
            };
            return question;
        }
    }
}