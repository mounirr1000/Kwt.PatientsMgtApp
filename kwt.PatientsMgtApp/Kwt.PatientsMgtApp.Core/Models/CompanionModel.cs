using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class CompanionModel : BaseEntity
    {
        public string CompanionCID { get; set; }
        public string CompanionFName { get; set; }
        public string CompanionMName { get; set; }
        public string CompanionLName { get; set; }
        public string PatientCID { get; set; }
        public string BankName { get; set; }
        public string bankCode { get; set; }
        public string IBan { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<System.DateTime> DateOut { get; set; }
        public bool? IsActive { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool? IsBeneficiary { get; set; }
        public string CompanionType{ get; set; }
    }
}
