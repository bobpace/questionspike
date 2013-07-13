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
            //annualEnrollmentQuestion.Answer = false;

            var drugCoverageQuestion =
                Question.ForAnswer<NewModel, PrescriptionDrugCoverage?>(x => x.CurrentlyEnrolledDrugCoverage);
            drugCoverageQuestion.QuestionText =
                "What type of medical or prescription drug coverage are you currently enrolled in?";

            //TODO: add unit test coverage demonstrating this usage as an option
//            drugCoverageQuestion.DisplayFormatter = new FlagsEnumDisplayFormatter<PrescriptionDrugCoverage>(
//                new Dictionary<PrescriptionDrugCoverage, string>
//                {
//                    {PrescriptionDrugCoverage.MedigapSelect, "Medigap Select"},
//                    {PrescriptionDrugCoverage.MedicareAdvantageOrMapd, "Medicare Advantage Or Mapd"},
//                    {PrescriptionDrugCoverage.MedicareOnly, "Medicare Only"},
//                    {PrescriptionDrugCoverage.NotEnrolledInMedicare, "Not Enrolled In Medicare"},
//                    {PrescriptionDrugCoverage.EmployerCoverage, "Employer Coverage"},
//                });

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
            birthdayQuestion.Answer = DateTime.Now;

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
    }
}