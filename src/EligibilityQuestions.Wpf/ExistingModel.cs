using System;

namespace EligibilityQuestions.Wpf
{
    [Flags]
    public enum CurrentCoverage
    {
        MedicareSupplementOrMedigap = 1,
        MedicareAdvantageWithDrugCoverage = 2,
        MedicareAdvantageWithoutDrugCoverage = 4,
        PrescriptionDrugPlan = 8
    }

    [Flags]
    public enum MilitaryBenefits
    {
        HasTricareForLife = 1,
        HasVaBenefits = 2
    }

    public class ExistingModel
    {
        public CurrentCoverage? CurrentCoverage { get; set; }
        public DateTime? EnrolledInMedicareSupplementDate { get; set; }
        public DateTime? EnrolledInMedicareAdvantageWithDrugCoverageDate { get; set; }
        public DateTime? EnrolledInMedicareAdvantageWithoutDrugCoverageDate { get; set; }
        public DateTime? EnrolledInPrescriptionDrugPlanDate { get; set; }

        public bool? HasEndStageRenalDisease { get; set; }
        public DateTime? EmployerCoverageEndDate { get; set; }
        public bool? IsEligibleForAnyOtherGroupCoverage { get; set; }
        public MilitaryBenefits? MilitaryBenefits { get; set; }
        public bool? HasLostOrIsLosingEmployerGroupCoverage { get; set; }
        public bool? WouldLikeToEnrollOrKeepOtherGroupCoverage { get; set; }
        public bool? HasMilitaryBenefits { get; set; }
        public bool? IsReceivingOrHasRecentlyStoppedReceivingMedicaidBenefits { get; set; }
        public bool? UsesTobacco { get; set; }
        public bool? HasIndividualMedicarePlansOutsideOfGroupPlan { get; set; }
    }
}