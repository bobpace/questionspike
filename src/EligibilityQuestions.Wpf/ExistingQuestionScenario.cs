using System;
using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions.Wpf
{
    public class ExistingQuestionScenario : QuestionScenario<ExistingModel>
    {
        public ExistingQuestionScenario()
        {
            var losingGroupCoverageQuestion =
                Question.ForAnswer<ExistingModel>(x => x.HasLostOrIsLosingEmployerGroupCoverage);
            losingGroupCoverageQuestion.QuestionText =
                "Did your employer provide a health plan which you lost in the last 90 days, or are losing in the near future?";

            var employerCoverageEndDateQuestion = Question.ForAnswer<ExistingModel>(x => x.EmployerCoverageEndDate);
            employerCoverageEndDateQuestion.QuestionText = "When did you, or will you, lose your employer coverage?";

            losingGroupCoverageQuestion.OnYes(x => employerCoverageEndDateQuestion);

            var hasPlansOutsideGroupPlanQuestion =
                Question.ForAnswer<ExistingModel>(x => x.HasIndividualMedicarePlansOutsideOfGroupPlan);
            hasPlansOutsideGroupPlanQuestion.QuestionText =
                "Are you currently enrolled in individual Medicare plans outside of a group plan?";

            var medigapDateQuestion = Question.ForAnswer<ExistingModel>(x => x.EnrolledInMedicareSupplementDate);
            medigapDateQuestion.QuestionText = "When did you enroll with Medigap?";
            var mapdDateQuestion = Question.ForAnswer<ExistingModel>(x => x.EnrolledInMedicareAdvantageWithDrugCoverageDate);
            mapdDateQuestion.QuestionText = "When did you enroll with Medicare Advantage with drug coverage (MAPD)?";
            var maDateQuestion = Question.ForAnswer<ExistingModel>(x => x.EnrolledInMedicareAdvantageWithoutDrugCoverageDate);
            maDateQuestion.QuestionText = "When did you enroll with Medicare Advantage without drug coverage (MA)?";
            var pdpDateQuestion = Question.ForAnswer<ExistingModel>(x => x.EnrolledInPrescriptionDrugPlanDate);
            pdpDateQuestion.QuestionText = "When did you enroll with Prescription drug plan (Part D)?";

            var currentCoverageQuestion = Question.ForAnswer<ExistingModel, CurrentCoverage?>(x => x.CurrentCoverage);
            currentCoverageQuestion.QuestionText = "(choose all that apply)";
            currentCoverageQuestion.SetExtraQuestions(new Dictionary<CurrentCoverage, Question>
            {
                {CurrentCoverage.MedicareSupplementOrMedigap, medigapDateQuestion},
                {CurrentCoverage.MedicareAdvantageWithDrugCoverage, mapdDateQuestion},
                {CurrentCoverage.MedicareAdvantageWithoutDrugCoverage, maDateQuestion},
                {CurrentCoverage.PrescriptionDrugPlan, pdpDateQuestion},
            });

            hasPlansOutsideGroupPlanQuestion.OnYes(x => currentCoverageQuestion);

            var eligibleForOtherGroupCoverageQuestion = Question.ForAnswer<ExistingModel>(x => x.IsEligibleForAnyOtherGroupCoverage);
            eligibleForOtherGroupCoverageQuestion.QuestionText = "Are you eligible for your spouse's or any other group coverage?";

            var wantsToKeepOtherGroupCoverage = Question.ForAnswer<ExistingModel>(x => x.WouldLikeToEnrollOrKeepOtherGroupCoverage);
            wantsToKeepOtherGroupCoverage.QuestionText = "Would you like to enroll in or keep that group plan?";

            eligibleForOtherGroupCoverageQuestion.OnYes(x => wantsToKeepOtherGroupCoverage);

            var hasMilitaryBenefitsQuestion = Question.ForAnswer<ExistingModel>(x => x.HasMilitaryBenefits);
            hasMilitaryBenefitsQuestion.QuestionText = "Are you currently using military benefits?";

            var militaryBenefitsQuestion = Question.ForAnswer<ExistingModel, MilitaryBenefits?>(x => x.MilitaryBenefits);

            hasMilitaryBenefitsQuestion.OnYes(x => militaryBenefitsQuestion);

            var usesTobaccoQuestion = Question.ForAnswer<ExistingModel>(x => x.UsesTobacco);
            usesTobaccoQuestion.QuestionText = "Have you used tobacco products in the last 24 months?";


            var endStageRenalDiseaseQuestion = Question.ForAnswer<ExistingModel>(x => x.HasEndStageRenalDisease);
            endStageRenalDiseaseQuestion.QuestionText = "Do you have End Stage Renal disease (kidney failure requireing dialysis)";

            var receivingMedicaidBenefitsQuestion = Question.ForAnswer<ExistingModel>(x => x.IsReceivingOrHasRecentlyStoppedReceivingMedicaidBenefits);
            receivingMedicaidBenefitsQuestion.QuestionText = "Are you now receiving or within the past two months have you stopped receiving Medicaid benefits?";


            //TODO: get the medical release statement back in here

            var questions = new Question[]
            {
                losingGroupCoverageQuestion,
                hasPlansOutsideGroupPlanQuestion,
                eligibleForOtherGroupCoverageQuestion,
                hasMilitaryBenefitsQuestion,
                usesTobaccoQuestion,
                endStageRenalDiseaseQuestion,
                receivingMedicaidBenefitsQuestion
            };

            Questions = questions;
            AllQuestions =
                questions.Concat(new Question[]
                {employerCoverageEndDateQuestion, currentCoverageQuestion, militaryBenefitsQuestion, medigapDateQuestion, maDateQuestion, mapdDateQuestion, pdpDateQuestion});
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new ExistingModel
                {
                    HasLostOrIsLosingEmployerGroupCoverage = true,
                    EmployerCoverageEndDate = new DateTime(2011, 12, 31),
                    HasIndividualMedicarePlansOutsideOfGroupPlan = true,
                    CurrentCoverage = CurrentCoverage.MedicareSupplementOrMedigap,
                    EnrolledInMedicareSupplementDate = new DateTime(2011, 12, 31),
                    HasMilitaryBenefits = true,
                    MilitaryBenefits = MilitaryBenefits.HasTricareForLife,
                    UsesTobacco = true,
                    HasEndStageRenalDisease = false,
                    IsReceivingOrHasRecentlyStoppedReceivingMedicaidBenefits = false
                };
            }
        }
    }
}