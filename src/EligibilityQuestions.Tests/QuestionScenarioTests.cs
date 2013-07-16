using System;
using System.Collections.Generic;
using System.Linq;
using EligibilityQuestions.Wpf;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class QuestionScenarioTests
    {
        private YesNoQuestion _likesBlueQuestion;
        private YesNoQuestion _likesGreenQuestion;
        private YesNoQuestion _likesRedQuestion;
        private DateTimeQuestion _birthdayQuestion;
        private YesNoQuestion[] _theQuestions;
        private MultipleSelectQuestion _choicesQuestion;

        [SetUp]
        public void Setup()
        {
            _likesBlueQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesBlue);
            _likesGreenQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesGreen);
            _likesRedQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesRed);
            _birthdayQuestion = Question.ForAnswer<ExampleModel>(x => x.BirthDay);
            _choicesQuestion = Question.ForAnswer<ExampleModel, ExampleFlagsEnum?>(x => x.MultipleSelectChoices);

            _likesBlueQuestion.OnNo(x => _birthdayQuestion);
            _likesGreenQuestion.OnYes(x => _likesRedQuestion);
            _likesGreenQuestion.OnNo(x => _choicesQuestion);

            _theQuestions = new[]
            {
                _likesBlueQuestion,
                _likesGreenQuestion
            };
        }

        [Test]
        public void questions_are_all_secondary_by_default()
        {
            new Question[]
            {_likesBlueQuestion, _likesGreenQuestion, _likesRedQuestion, _birthdayQuestion, _choicesQuestion}
                .All(x => x.Rank == QuestionRank.Secondary)
                .ShouldBeTrue();
        }

        [Test]
        public void top_level_questions_passed_into_a_scenario_become_primary()
        {
            new ExampleScenario(_theQuestions);

            _theQuestions
                .All(x => x.Rank == QuestionRank.Primary)
                .ShouldBeTrue();
            _likesRedQuestion.Rank.ShouldEqual(QuestionRank.Secondary);
            _birthdayQuestion.Rank.ShouldEqual(QuestionRank.Secondary);
            _choicesQuestion.Rank.ShouldEqual(QuestionRank.Secondary);
        }

        [Test]
        public void can_build_model_from_set_of_answered_questions()
        {
            var now = DateTime.Now;
            _likesBlueQuestion.Answer = false;
            _birthdayQuestion.Answer = now;
            _likesGreenQuestion.Answer = false;
            const ExampleFlagsEnum choice = ExampleFlagsEnum.ChoiceOne | ExampleFlagsEnum.ChoiceTwo;
            _choicesQuestion.Answer = choice;

            var scenario = new ExampleScenario(_theQuestions);
            var result = scenario.BuildModel();

            result.LikesBlue.Value.ShouldBeFalse();
            result.LikesGreen.Value.ShouldBeFalse();
            result.BirthDay.ShouldEqual(now);
            result.MultipleSelectChoices.ShouldEqual(choice);
        }

        [Test]
        public void only_answered_questions_that_are_visible_are_put_onto_the_model()
        {
            var now = DateTime.Now;
            _likesBlueQuestion.Answer = true;
            _likesGreenQuestion.Answer = true;
            _birthdayQuestion.Answer = now;
            _choicesQuestion.Answer = ExampleFlagsEnum.ChoiceThree;

            var scenario = new ExampleScenario(_theQuestions);
            var result = scenario.BuildModel();

            //these questions get their answers as expected
            result.LikesBlue.Value.ShouldBeTrue();
            result.LikesGreen.Value.ShouldBeTrue();
            //these questions are not in the branch of the currently answered question above it
            //and are not shown, therefore not answered
            result.BirthDay.ShouldBeNull();
            result.MultipleSelectChoices.ShouldBeNull();
        }

        [Test]
        public void can_prepopulate_data_into_a_question_scenario()
        {
            var now = DateTime.Now;
            var scenario = new ExampleScenario(_theQuestions);
            const ExampleFlagsEnum choice = ExampleFlagsEnum.ChoiceOne | ExampleFlagsEnum.ChoiceTwo;

            scenario.SetAnswersFromModel(new ExampleModel
            {
                LikesBlue = false,
                LikesGreen = false,
                LikesRed = false,
                MultipleSelectChoices = choice,
                BirthDay = now
            });

            var model = scenario.BuildModel();
            model.LikesBlue.Value.ShouldBeFalse();
            model.LikesGreen.Value.ShouldBeFalse();
            model.BirthDay.ShouldEqual(now);
            //likes red ends up being null on purpose because likes green is false and
            //the likes red question only appears if likes green is true
            model.LikesRed.ShouldBeNull();
            model.MultipleSelectChoices.ShouldEqual(choice);
        }

        [Test]
        public void reset_reverts_all_fields_to_default_values()
        {
            var scenario = new ExampleScenario(_theQuestions);

            scenario.SetAnswersFromModel(new ExampleModel
            {
                LikesBlue = true,
                LikesGreen = true,
                LikesRed = true,
            });

            scenario.Reset();

            var model = scenario.BuildModel();
            model.LikesBlue.ShouldBeNull();
            model.LikesGreen.ShouldBeNull();
            model.LikesRed.ShouldBeNull();
        }


        public class ExampleModel
        {
            public bool? LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public bool? LikesRed { get; set; }
            public ExampleFlagsEnum? MultipleSelectChoices { get; set; }
            public DateTime? BirthDay { get; set; }
        }

        [Flags]
        public enum ExampleFlagsEnum
        {
            ChoiceOne = 1,
            ChoiceTwo = 2,
            ChoiceThree = 4
        }

        public class ExampleScenario : QuestionScenario<ExampleModel>
        {
            public ExampleScenario(IEnumerable<Question> questions)
            {
                Questions = questions;
            }
        }
    }
}