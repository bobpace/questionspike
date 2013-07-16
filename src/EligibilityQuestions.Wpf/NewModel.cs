using System;
using System.Linq;
using System.Reflection;
using FubuCore;
using System.Collections.Generic;

namespace EligibilityQuestions.Wpf
{
    [Flags]
    public enum PrescriptionDrugCoverage
    {
        Medigap = 1,
        MedigapSelect = 2,
        MedicareAdvantageOrMAPD = 4,
        PDP = 8,
        MedicareOnly = 16,
        NotEnrolledInMedicare = 32,
        EmployerCoverage = 64,
        None = 128,
    }

    [Flags]
    public enum DiscussCoverage
    {
        Medical = 1,
        PDP = 2,
        DentalOrVision = 4
    }

    [Flags]
    public enum PromptedCallReason
    {
        RateIncrease = 1,
        ChangeInHeathStatus=2,
        MyPlanIsBeingDiscontinued=4,
        WouldLikeToComparePlanOptions=8,
        CustomerServiceQuestion=16
    }

    public class NewModel
    {
        public bool? WithinAnnualEnrollmentPeriod { get; set; }
        public PrescriptionDrugCoverage? CurrentlyEnrolledDrugCoverage { get; set; }
        public DiscussCoverage? DiscussCoverage { get; set; }
        public PromptedCallReason? PromptedCallReason { get; set; }
    }
}