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
        //public const string Other = "Other";
        //public const string Other = "Other";
        //public const string Other = "Other";
        
    }
}
