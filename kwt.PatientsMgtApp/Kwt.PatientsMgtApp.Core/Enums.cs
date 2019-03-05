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
    }
}
