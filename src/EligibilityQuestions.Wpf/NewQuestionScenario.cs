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

            var enrolledInMedigapQuestion = Question.ForAnswer<NewModel>(x => x.EnrolledInMedigapDate);
            enrolledInMedigapQuestion.QuestionText = "When did you enroll in medigap?";
            var enrolledInPdpQuestion = Question.ForAnswer<NewModel>(x => x.EnrolledInPdpDate);
            enrolledInPdpQuestion.QuestionText = "When did you enroll in pdp?";

            var questionMap = new Dictionary<PrescriptionDrugCoverage, Question>
            {
                {PrescriptionDrugCoverage.Medigap, enrolledInMedigapQuestion},
                {PrescriptionDrugCoverage.PDP, enrolledInPdpQuestion}
            };

            //annualEnrollmentQuestion.OnYes(x => drugCoverageQuestion);

            drugCoverageQuestion.SetExtraQuestions(questionMap);

            var birthdayQuestion = Question.ForAnswer<NewModel>(x => x.Birthday);
            birthdayQuestion.QuestionText = "When is your birthday?";

            var greenQuestion = Question.ForAnswer<NewModel>(x => x.LikesGreen);
            greenQuestion.QuestionText = "Do you like green?";

            var yellowQuestion = Question.ForAnswer<NewModel>(x => x.LikesYellow);
            yellowQuestion.QuestionText = "Do you like yellow?";

            var purpleQuestion = Question.ForAnswer<NewModel>(x => x.LikesPurple);
            purpleQuestion.QuestionText = "Do you like purple?";

            //drugCoverageQuestion.OnNext(x => birthdayQuestion);

            birthdayQuestion.OnNext(x => greenQuestion);

            greenQuestion.OnYes(x => yellowQuestion);
            greenQuestion.OnNo(x => purpleQuestion);

            var questions = new Question[]
            {
                annualEnrollmentQuestion,
                drugCoverageQuestion,
                birthdayQuestion
            };

            Questions = questions;
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new NewModel
                {
                    Birthday = DateTime.Now,
                    WithinAnnualEnrollmentPeriod = true,
                    CurrentlyEnrolledDrugCoverage = PrescriptionDrugCoverage.MedicareAdvantageOrMapd | PrescriptionDrugCoverage.PDP,
                    LikesGreen = true,
                    EnrolledInPdpDate = DateTime.Now
                };
            }
        }
    }
}