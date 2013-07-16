using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions.Wpf
{
    public class YesNoDemoScenario : QuestionScenario<YesNoDemoModel>
    {
        public YesNoDemoScenario()
        {
            var likesBlueQuestion = Question.ForAnswer<YesNoDemoModel>(x => x.LikesBlue);
            likesBlueQuestion.QuestionText = "Do you like blue?";

            var likesRedQuestion = Question.ForAnswer<YesNoDemoModel>(x => x.LikesRed);
            likesRedQuestion.QuestionText = "Do you like red?";

            var likesGreenQuestion = Question.ForAnswer<YesNoDemoModel>(x => x.LikesGreen);
            likesGreenQuestion.QuestionText = "Do you like green?";

            var likesYellowQuestion = Question.ForAnswer<YesNoDemoModel>(x => x.LikesYellow);
            likesYellowQuestion.QuestionText = "Do you like yellow?";

            var likesOrangeQuestion = Question.ForAnswer<YesNoDemoModel>(x => x.LikesOrange);
            likesOrangeQuestion.QuestionText = "Do you like orange?";

            likesBlueQuestion.OnYes(x => likesRedQuestion);
            likesBlueQuestion.OnNo(x => likesGreenQuestion);

            likesGreenQuestion.OnYes(x => likesYellowQuestion);

            likesRedQuestion.OnYes(x => likesOrangeQuestion);

            var questions = new Question[]
            {
                likesBlueQuestion
            };

            Questions = questions;
            AllQuestions = questions.Concat(new[] {likesRedQuestion, likesGreenQuestion, likesYellowQuestion, likesOrangeQuestion});
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new YesNoDemoModel
                {
                    LikesBlue = true,
                    LikesRed = true,
                    LikesOrange = false
                };
                yield return new YesNoDemoModel
                {
                    LikesBlue = false,
                    LikesGreen = true,
                    LikesYellow = true
                };
            }
        }
    }

    public class YesNoDemoModel
    {
        public bool? LikesBlue { get; set; }
        public bool? LikesGreen { get; set; }
        public bool? LikesYellow { get; set; }
        public bool? LikesRed { get; set; }
        public bool? LikesOrange { get; set; }
    }
}