using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientHistoryModel:BaseEntity
    {

        public string PatientCID { get; set; }
        public string PatientFName { get; set; }
        public string PatientMName { get; set; }
        public string PatientLName { get; set; }
        public string KWTphone { get; set; }
        public string USphone { get; set; }
        public string Hospital { get; set; }
        public string Doctor { get; set; }
        public string Agency { get; set; }
        public Nullable<int> BankID { get; set; }
        public string Iban { get; set; }
        public Nullable<System.DateTime> FirstApptDate { get; set; }
        public String FirstApptDateFormatted { get { return String.Format("{0:d}", FirstApptDate); } }
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        public String EndTreatDateFormatted { get { return String.Format("{0:d}", EndTreatDate); } }
        public Nullable<bool> IsBeneficiary { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public System.DateTime OldCreatedDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string PrimaryCompanionCid { get; set; }

    }
}
