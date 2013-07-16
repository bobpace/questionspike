using System;
using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions.Wpf
{
    public class MultipleSelectDemoScenario : QuestionScenario<MultipleSelectDemoModel>
    {
        public MultipleSelectDemoScenario()
        {
            var clothingChoiceQuestion = Question.ForAnswer<MultipleSelectDemoModel, TypeOfClothing?>(x => x.ClothingChoice);
            clothingChoiceQuestion.QuestionText = "What type of clothing are you interested in?";

            var shirtSizeQuestion = Question.ForAnswer<MultipleSelectDemoModel, Size?>(x => x.ShirtSize);
            shirtSizeQuestion.QuestionText = "What size shirt do you wear?";
            var pantsSizeQuestion = Question.ForAnswer<MultipleSelectDemoModel, Size?>(x => x.PantsSize);
            pantsSizeQuestion.QuestionText = "What size pants do you wear?";
            var jacketSizeQuestion = Question.ForAnswer<MultipleSelectDemoModel, Size?>(x => x.JacketSize);
            jacketSizeQuestion.QuestionText = "What size jacket do you wear?";

            clothingChoiceQuestion.SetExtraQuestions(new Dictionary<TypeOfClothing, Question>
            {
                {TypeOfClothing.Shirt, shirtSizeQuestion},
                {TypeOfClothing.Pants, pantsSizeQuestion},
                {TypeOfClothing.Jacket, jacketSizeQuestion},
            });

            var questions = new Question[]
            {
                clothingChoiceQuestion
            };

            Questions = questions;
            AllQuestions = questions.Concat(new[] {shirtSizeQuestion, pantsSizeQuestion, jacketSizeQuestion});
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new MultipleSelectDemoModel
                {
                    ClothingChoice = TypeOfClothing.Shirt,
                    ShirtSize = Size.Medium
                };

                yield return new MultipleSelectDemoModel
                {
                    ClothingChoice = TypeOfClothing.Shirt | TypeOfClothing.Pants,
                    ShirtSize = Size.Medium,
                    PantsSize = Size.Large
                };
            }
        }
    }

    public class MultipleSelectDemoModel
    {
        public TypeOfClothing? ClothingChoice { get; set; }
        public Size? ShirtSize { get; set; }
        public Size? PantsSize { get; set; }
        public Size? JacketSize { get; set; }
    }

    [Flags]
    public enum TypeOfClothing
    {
        Shirt = 1,
        Pants = 2,
        Jacket = 4
    }

    [Flags]
    public enum Size
    {
        Small = 1,
        Medium = 2,
        Large = 4
    }
}