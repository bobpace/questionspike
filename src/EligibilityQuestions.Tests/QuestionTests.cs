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
            likesBlueQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesBlue);
            likesBlueQuestion.Answer = false;

            likesGreenQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            likesGreenQuestion.Answer = true;

            var now = DateTime.Now;

            birthdayQuestion = Question.ForAnswer<EndResultModel>(x => x.BirthDay);
            birthdayQuestion.Answer = now;

            likesBlueQuestion.OnNo(x => likesGreenQuestion);
            likesGreenQuestion.OnYes(x => birthdayQuestion);

            var builder = new ModelBuilder<EndResultModel>(likesBlueQuestion);
            var result = builder.BuildModel();

            result.LikesBlue.ShouldBeFalse();
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