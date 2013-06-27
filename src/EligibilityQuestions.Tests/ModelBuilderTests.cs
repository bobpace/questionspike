using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class ModelBuilderTests
    {
        private YesNoQuestion likesBlueQuestion;
        private YesNoQuestion likesGreenQuestion;
        private DateTimeQuestion birthdayQuestion;
        private DateTime now;
        private ModelBuilder<EndResultModel> theModelBuilder;

        [SetUp]
        public void Setup()
        {
            likesBlueQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesBlue);
            likesGreenQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            birthdayQuestion = Question.ForAnswer<EndResultModel>(x => x.BirthDay);
            likesBlueQuestion.OnNo(x => likesGreenQuestion);
            likesGreenQuestion.OnYes(x => birthdayQuestion);
            now = DateTime.Now;
            theModelBuilder = new ModelBuilder<EndResultModel>(likesBlueQuestion);
        }

        [Test]
        public void can_build_model_from_set_of_answered_questions()
        {
            likesBlueQuestion.Answer = false;
            likesGreenQuestion.Answer = true;
            birthdayQuestion.Answer = now;

            var result = theModelBuilder.BuildModel();

            result.LikesBlue.Value.ShouldBeFalse();
            result.LikesGreen.Value.ShouldBeTrue();
            result.BirthDay.ShouldEqual(now);
        }

        [Test]
        public void only_answered_questions_that_are_visible_are_put_onto_the_model()
        {
            likesBlueQuestion.Answer = false;
            likesGreenQuestion.Answer = false;
            birthdayQuestion.Answer = now;

            var result = theModelBuilder.BuildModel();

            result.LikesBlue.Value.ShouldBeFalse();
            result.LikesGreen.Value.ShouldBeFalse();
            result.BirthDay.ShouldBeNull();
        }

        public class EndResultModel
        {
            public bool? LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public DateTime? BirthDay { get; set; }
        }
    }

}