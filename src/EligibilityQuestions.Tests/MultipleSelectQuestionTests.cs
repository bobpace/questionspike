using System;
using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class MultipleSelectQuestionTests
    {
        private MultipleSelectQuestion _question;
        private DateTimeQuestion _birthdayQuestion;
        private YesNoQuestion _likesBlueQuestion;

        [SetUp]
        public void Setup()
        {
            _question = Question.ForAnswer<EndResultModel, Choices?>(x => x.MyChoice);
            _birthdayQuestion = Question.ForAnswer<EndResultModel>(x => x.Birthday);
            _likesBlueQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesBlue);
            _question.SetExtraQuestions(new Dictionary<Choices, Question>
            {
                {Choices.One, _birthdayQuestion},
                {Choices.Three, _likesBlueQuestion}
            });
        }

        [Test]
        public void can_enable_additional_questions_per_each_flags_enum_choice_example_one()
        {
            _question.Answer = Choices.One;

            var nextQuestions = _question.NextQuestions;
            nextQuestions.ShouldContain(_birthdayQuestion);
        }

        [Test]
        public void can_enable_additional_questions_per_each_flags_enum_choice_example_two()
        {
            _question.Answer = Choices.One | Choices.Three;

            var nextQuestions = _question.NextQuestions;
            nextQuestions.ShouldHaveTheSameElementsAs(_birthdayQuestion, _likesBlueQuestion);
        }

        [Test]
        public void answered_elements_retrieves_next_questions_that_may_have_been_toggled_on_due_to_multiple_select_choices()
        {
            _question.Answer = Choices.One;
            _birthdayQuestion.Answer = DateTime.Today;

            _question.AnsweredQuestions()
                .ShouldHaveTheSameElementsAs(new Question[]{_question, _birthdayQuestion});
        }

        [Test]
        public void answered_elements_still_continues_past_next_questions_into_normal_next_question_flow()
        {
            var normalNextQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            _question.OnNext(x => normalNextQuestion);

            _question.Answer = Choices.One | Choices.Three;

            _question.AnsweredQuestions()
                .ShouldHaveTheSameElementsAs(new Question[]{_question, _birthdayQuestion, _likesBlueQuestion, normalNextQuestion});
        }

        public class EndResultModel
        {
            public Choices MyChoice { get; set; }
            public DateTime? Birthday { get; set; }
            public bool? LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
        }

        [Flags]
        public enum Choices
        {
            One = 1,
            Two = 2,
            Three = 4
        }
    }
}