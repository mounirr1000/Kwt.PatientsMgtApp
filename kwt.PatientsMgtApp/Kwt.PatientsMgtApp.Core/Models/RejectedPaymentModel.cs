using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class RejectedPaymentModel : BaseEntity
    {

        public int RejectionID { get; set; }
        public int PaymentID { get; set; }

        [DisplayName("Select Rejection Reason")]
        public int RejectionReasonID { get; set; }
        public Nullable<System.DateTime> RejectedDate { get; set; }

        [DisplayName("Notes For Rejection")]
        public string RejectionNotes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //public virtual Payment Payment { get; set; }
        //public virtual RejectionReason RejectionReason { get; set; }
    }
}
