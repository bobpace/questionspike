using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class QuestionGroupingTests
    {
        private YesNoQuestion likesBlueQuestion;
        private YesNoQuestion likesGreenQuestion;
        private YesNoQuestion likesRedQuestion;
        private YesNoQuestion theQuestion;
        private DateTimeQuestion birthdayQuestion;

        [SetUp]
        public void Setup()
        {
            likesBlueQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesBlue);
            likesBlueQuestion.Answer = true;

            likesGreenQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            likesGreenQuestion.Answer = true;

            likesRedQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            likesRedQuestion.Answer = true;

            birthdayQuestion = Question.ForAnswer<EndResultModel>(x => x.Birthday);
            birthdayQuestion.Answer = DateTime.Now;

            likesBlueQuestion
                .OnYes(
                    x => birthdayQuestion.OnNext(
                        b => likesGreenQuestion.OnYes(
                            m => likesRedQuestion))
                );

            theQuestion = likesBlueQuestion;
        }

        [Test]
        public void question_grouping_advances_to_next_question_when_answered()
        {
            theQuestion.Accessor.Name.ShouldEqual("LikesBlue");
            theQuestion.Answer = true;
            var nextQuestion = theQuestion.NextQuestion;
            nextQuestion.Accessor.Name.ShouldEqual("Birthday");
            nextQuestion.Answer = DateTime.Now;
            nextQuestion = nextQuestion.NextQuestion;
            nextQuestion.Accessor.Name.ShouldEqual("LikesGreen");
            nextQuestion.Answer = true;
            nextQuestion = nextQuestion.NextQuestion;
            nextQuestion.Accessor.Name.ShouldEqual("LikesRed");
        }

        [Test]
        public void question_grouping_eventually_runs_out_of_questions()
        {
            theQuestion.Accessor.Name.ShouldEqual("LikesBlue");
            theQuestion.Answer = false;
            var nextQuestion = theQuestion.NextQuestion;
            nextQuestion.ShouldBeNull();
        }


        public class EndResultModel
        {
            public bool LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public bool LikesRed { get; set; }
            public DateTime Birthday { get; set; }
        }
    }
}