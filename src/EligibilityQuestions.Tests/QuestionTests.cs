using System;
using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class QuestionTests
    {
        private YesNoQuestion likesBlueQuestion;
        private YesNoQuestion likesGreenQuestion;
        private DateTimeQuestion birthdayQuestion;

        [Test]
        public void questions_can_target_properties_on_result_model()
        {
            likesBlueQuestion = new YesNoQuestion();
            likesBlueQuestion.ForAnswer<EndResultModel>(x => x.LikesBlue);
            likesBlueQuestion.Answer = true;

            likesGreenQuestion = new YesNoQuestion();
            likesGreenQuestion.ForAnswer<EndResultModel>(x => x.LikesGreen);
            likesGreenQuestion.Answer = true;

            var now = DateTime.Now;

            birthdayQuestion = new DateTimeQuestion();
            birthdayQuestion.ForAnswer<EndResultModel>(x => x.BirthDay);
            birthdayQuestion.Answer = now;

            var questions = new Question[]
            {
                likesBlueQuestion, likesGreenQuestion, birthdayQuestion
            };

            var builder = new ModelBuilder<EndResultModel>(questions);
            var result = builder.BuildModel();

            result.LikesBlue.ShouldBeTrue();
            result.LikesGreen.Value.ShouldBeTrue();
            result.BirthDay.ShouldEqual(now);
        }

        public class EndResultModel
        {
            public bool LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public DateTime? BirthDay { get; set; }
        }
    }

}