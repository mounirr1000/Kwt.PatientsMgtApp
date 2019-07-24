using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class RejectionReasonModel : BaseEntity
    {
        public int RejectionReasonID { get; set; }
        public string RejectionReason1 { get; set; }

       
        //public virtual ICollection<RejectedPayment> RejectedPayments { get; set; }
    }
}
