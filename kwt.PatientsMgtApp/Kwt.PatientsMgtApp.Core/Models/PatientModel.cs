using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientModel: BaseEntity
    {
        public string PatientCID { get; set; }
        public string PatientFName { get; set; }
        public string PatientMName { get; set; }
        public string PatientLName { get; set; }
        public string KWTPhone { get; set; }
        public string USPhone { get; set; }
        public string Hospital { get; set; }
        public string Doctor { get; set; }
        public string Agency { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Iban { get; set; }
        public Nullable<System.DateTime> FirstApptDAte { get; set; }
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        //public Nullable<decimal> PatientRate { get; set; }
        public Nullable<bool> IsBeneficiary { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }

    }
}
