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
        private YesNoQuestion[] _theQuestions;

        [SetUp]
        public void Setup()
        {
            _likesBlueQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesBlue);
            _likesGreenQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesGreen);
            _likesRedQuestion = Question.ForAnswer<ExampleModel>(x => x.LikesRed);

            _likesGreenQuestion.OnYes(x => _likesRedQuestion);

            _theQuestions = new[]
            {
                _likesBlueQuestion,
                _likesGreenQuestion
            };
        }

        [Test]
        public void questions_are_all_secondary_by_default()
        {
            new[] {_likesBlueQuestion, _likesGreenQuestion, _likesRedQuestion}
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
        }

        public class ExampleModel
        {
            public bool LikesBlue { get; set; }
            public bool LikesGreen { get; set; }
            public bool LikesRed { get; set; }
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