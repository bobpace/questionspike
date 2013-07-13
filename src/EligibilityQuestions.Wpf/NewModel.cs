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
        MedicareAdvantageOrMapd = 4,
        PDP = 8,
        MedicareOnly = 16,
        NotEnrolledInMedicare = 32,
        EmployerCoverage = 64,
        None = 128,
    }

    public class NewModel
    {
        public bool? WithinAnnualEnrollmentPeriod { get; set; }
        public PrescriptionDrugCoverage? CurrentlyEnrolledDrugCoverage { get; set; }
        public DateTime? EnrolledInMedigapDate { get; set; }
        public DateTime? EnrolledInPdpDate { get; set; }
        public bool? LikesGreen { get; set; }
        public bool? LikesYellow { get; set; }
        public bool? LikesPurple { get; set; }
        public DateTime? Birthday { get; set; }
    }
}