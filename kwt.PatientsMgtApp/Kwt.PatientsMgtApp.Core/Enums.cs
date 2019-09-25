using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core
{
    public class Enums
    {
        public enum CompanionType
        {
            Primary = 1,
            Secondary = 2,
            Other=3
        }

        public enum MeetingParticipationStatus
        {
            Anticipated = 1,
            Confirmed = 2
        }

        public enum PayRates
        {
            PatientRate = 75,
            CompanionRate = 25,
            Other = 0
        }

        public enum ReportType
        {
            Kuwait = 1,// bank
            Archive = 2,
            Details = 3,
            Ministry = 4,
            Statistical = 5,
            Statistical2 = 6,
        }
        public enum PaymentType
        {
            Regular = 1,
            Correction = 2,
            Other = 3,
            Adjustment=4
        }
    }
}
