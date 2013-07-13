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

    public class ExistingModel
    {
        public CurrentCoverage CurrentCoverage { get; set; }
        public DateTime? EnrolledInMedicareSupplementDate { get; set; }
        public DateTime? EnrolledInMedicareAdvantageWithDrugCoverageDate { get; set; }
        public DateTime? EnrolledInMedicareAdvantageWithoutDrugCoverageDate { get; set; }
        public DateTime? EnrolledInPrescriptionDrugPlanDate { get; set; }

        public bool? HasEndStageRenalDisease { get; set; }
        public DateTime? DisabledOnDate { get; set; }
        public DateTime? MoveDate { get; set; }
        public DateTime? SkilledNursingFacilityMoveDate { get; set; }
        public int? SkilledNursingFacilityMoveType { get; set; }
        public DateTime? EmployerCoverageEndDate { get; set; }
        public bool? IsEligibleForAnyOtherGroupCoverage { get; set; }
        public bool? IsMoveWithinSameCounty { get; set; }
        public bool? HasTricareForLife { get; set; }
        public bool? HasVaBenefits { get; set; }
        public bool? HasLostOrIsLosingEmployerGroupCoverage { get; set; }
        public bool? WouldLikeToEnrollOrKeepOtherGroupCoverage { get; set; }
        public bool? HasMilitaryBenefits { get; set; }
        public bool? IsReceivingOrHasRecentlyStoppedReceivingMedicaidBenefits { get; set; }
        public bool? HasResidedInASkilledNursingFacility { get; set; }
        public bool? HasMovedInLastTwoMonths { get; set; }
        public bool StillLivesInTheFacility { get; set; }
        public bool? UsesTobacco { get; set; }
        public bool? HasIndividualMedicarePlansOutsideOfGroupPlan { get; set; }
        public DateTime? PregnancyDueDate { get; set; }
        public DateTimeOffset? EligibilityAnswersLastModifiedAtTime { get; set; }
        public string EmployeeFullName { get; set; }
        public bool IsStudent { get; set; }
        public bool DiscardInvalidItems { get; set; }
        public bool IsSaving { get; set; }
        public bool MedicareAdvantageCoverageSelected { get; set; }
        public bool MapdCoverageSelected { get; set; }
        public bool PartDCoverageSelected { get; set; }
        public bool MedigapCoverageSelected { get; set; }
        public bool RequiresMedicalReleaseStatement { get; set; }
    }
}