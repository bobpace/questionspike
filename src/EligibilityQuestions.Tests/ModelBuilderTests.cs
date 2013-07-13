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
        private MultipleSelectQuestion choicesQuestion;
        private DateTime now;
        private Question[] questions;

        [SetUp]
        public void Setup()
        {
            likesBlueQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesBlue);
            likesGreenQuestion = Question.ForAnswer<EndResultModel>(x => x.LikesGreen);
            birthdayQuestion = Question.ForAnswer<EndResultModel>(x => x.BirthDay);
            choicesQuestion = Question.ForAnswer<EndResultModel, ExampleFlagsEnum?>(x => x.MultipleSelectChoices);

            likesBlueQuestion.OnNo(x => likesGreenQuestion);
            likesGreenQuestion.OnYes(x => birthdayQuestion);
            birthdayQuestion.OnNext(x => choicesQuestion);

            questions = new[] {likesBlueQuestion};

            now = DateTime.Now;
        }

        [Test]
        public void can_build_model_from_set_of_answered_questions()
        {
            likesBlueQuestion.Answer = false;
            likesGreenQuestion.Answer = true;
            birthdayQuestion.Answer = now;
            const ExampleFlagsEnum choice = ExampleFlagsEnum.ChoiceOne | ExampleFlagsEnum.ChoiceTwo;
            choicesQuestion.Answer = choice;

            var result = ModelBuilder<EndResultModel>.BuildModelFrom(questions);

            result.LikesBlue.Value.ShouldBeFalse();
            result.LikesGreen.Value.ShouldBeTrue();
            result.BirthDay.ShouldEqual(now);
            result.MultipleSelectChoices.ShouldEqual(choice);
        }

        [Test]
        public void only_answered_questions_that_are_visible_are_put_onto_the_model()
        {
            likesBlueQuestion.Answer = false;
            likesGreenQuestion.Answer = false;
            birthdayQuestion.Answer = now;
            choicesQuestion.Answer = ExampleFlagsEnum.ChoiceThree;

            var result = ModelBuilder<EndResultModel>.BuildModelFrom(questions);

            //these questions get their answers as expected
            result.LikesBlue.Value.ShouldBeFalse();
            result.LikesGreen.Value.ShouldBeFalse();
            //these questions are not in the branch of the currently answered question above it
            //and are not shown, therefore not answered
            result.BirthDay.ShouldBeNull();
            result.MultipleSelectChoices.ShouldBeNull();
        }

        public class EndResultModel
        {
            public bool? LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public DateTime? BirthDay { get; set; }
            public ExampleFlagsEnum? MultipleSelectChoices { get; set; }
        }

        [Flags]
        public enum ExampleFlagsEnum
        {
            ChoiceOne = 1,
            ChoiceTwo = 2,
            ChoiceThree = 4
        }
    }

}