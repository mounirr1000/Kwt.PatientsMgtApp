using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core
{
    public class Constants
    {
        public const int NUMBER_OF_DAYS_BEFORE_NEXT_PAYMENT=15;
        public const string FINDING_DOC_LIB_NAME = "Finding Documents";
        public const string MOU_DOC_LIB_NAME = "MOU Documents";
        public const string EA_DOC_LIB_NAME = "Early Alert Documents";
        public const string DRAFT_REPORTS_LIB_NAME = "Draft Reports";
        public const string PUBLISHED_REPORTS_LIB_NAME = "Published Reports";
        public const string OTHER_DOCS_LIB_NAME = "Other Documents";
    }
    public class Roles
    {
        public const string Admin = "Admin";
        public const string Accountant = "Accountant";
        public const string User = "User";
        public const string SuperAdmin = "Super Admin";
        public const string Auditor = "Auditor";
        public const string Editor = "Editor";
        public const string Medical = "Medical";
        public const string Other = "Other";
    }
    public class PatientFolders
    {
        
        public const string Allowances = "ALLOWANCES";
        public const string Appointments = "APPOINTMENTS";
        public const string CompanionInfo = "COMPANION INFO";
        public const string FuneralArrangement = "FUNERAL ARRANGEMENT";
        public const string GuaranteeLetters = "GUARANTEE LETTERS";
        public const string IncomingMph = "INCOMING MPH";
        public const string MedicalReports = "MEDICAL REPORTS";
        public const string OutgoingMph = "OUTGOING MPH";
        public const string PatientInfo = "PATIENT INFO";
        public const string ProgressReports = "PROGRESS REPORTS";
        public const string TicketingTransportation = "TICKETING & TRANSPORTATION";
    }
}
