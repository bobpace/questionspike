using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class QuestionGroupingTests
    {
        private YesNoQuestion<EndResultModel> likesBlueQuestion;
        private YesNoQuestion<EndResultModel> likesGreenQuestion;
        private YesNoQuestion<EndResultModel> likesRedQuestion;
        private YesNoQuestion<EndResultModel> theQuestion;
        private DateTimeQuestion<EndResultModel> birthdayQuestion;

        [SetUp]
        public void Setup()
        {
            likesBlueQuestion = new YesNoQuestion<EndResultModel>(x => x.LikesBlue)
            {
                Answer = true
            };

            likesGreenQuestion = new YesNoQuestion<EndResultModel>(x => x.LikesGreen)
            {
                Answer = true
            };

            likesRedQuestion = new YesNoQuestion<EndResultModel>(x => x.LikesRed)
            {
                Answer = true
            };

            birthdayQuestion = new DateTimeQuestion<EndResultModel>(x => x.Birthday)
            {
                Answer = DateTime.Now
            };

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
            var nextQuestion = theQuestion.AnswerQuestion(true); //i like blue
            nextQuestion.Accessor.Name.ShouldEqual("Birthday");
            nextQuestion = nextQuestion.AnswerQuestion(DateTime.Now);
            nextQuestion.Accessor.Name.ShouldEqual("LikesGreen");
            nextQuestion = nextQuestion.AnswerQuestion(true);
            nextQuestion.Accessor.Name.ShouldEqual("LikesRed");
        }

        [Test]
        public void question_grouping_eventually_runs_out_of_questions()
        {
            theQuestion.Accessor.Name.ShouldEqual("LikesBlue");
            var nextQuestion = theQuestion.AnswerQuestion(false); //i dont like blue
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