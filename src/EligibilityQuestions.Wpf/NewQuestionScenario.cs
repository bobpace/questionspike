using System;
using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    public class NewQuestionScenario : QuestionScenario<NewModel>
    {
        public NewQuestionScenario()
        {
            var annualEnrollmentQuestion = Question.ForAnswer<NewModel>(x => x.WithinAnnualEnrollmentPeriod);
            annualEnrollmentQuestion.QuestionText = "Are you within the annual enrollment period?";

            var drugCoverageQuestion =
                Question.ForAnswer<NewModel, PrescriptionDrugCoverage?>(x => x.CurrentlyEnrolledDrugCoverage);
            drugCoverageQuestion.QuestionText =
                "What type of medical or prescription drug coverage are you currently enrolled in?";

            var discussCoverageQuestion =
                Question.ForAnswer<NewModel, DiscussCoverage?>(x => x.DiscussCoverage);
            discussCoverageQuestion.QuestionText =
                "What type of coverage would you like to discuss today?";

            var promptedCallQuestion =
                Question.ForAnswer<NewModel, PromptedCallReason?>(x => x.PromptedCallReason);
            promptedCallQuestion.QuestionText = "What prompted your call today?";

            var questions = new Question[]
            {
                annualEnrollmentQuestion,
                drugCoverageQuestion,
                discussCoverageQuestion,
                promptedCallQuestion
            };

            //TODO: finish settuping up new question scenario
            //get certain question groups to hide/show based on answers to other question groups
            Questions = questions;
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new NewModel
                {
                    WithinAnnualEnrollmentPeriod = true,
                    CurrentlyEnrolledDrugCoverage = PrescriptionDrugCoverage.MedicareAdvantageOrMAPD | PrescriptionDrugCoverage.PDP,
                };
            }
        }
    }
}