using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayeeModel
    {
        public int PayeeID { get; set; }
        [DisplayName("Payee")]
        public string PayeeName { get; set; }
        [DisplayName("Street Address")]
        public string PayeeStreetAddress { get; set; }
        [DisplayName("City")]
        public string PayeeCity { get; set; }
        [DisplayName("State")]
        public string PayeeState { get; set; }
        [DisplayName("Zip Code")]
        public string PayeeZipcode { get; set; }

        public string CityStateZipAddress { get { return PayeeCity + ",  "+PayeeState+",  "+ PayeeZipcode; } }
        [DisplayName("Phone Number")]
        public string PayeePhone { get; set; }
        [DisplayName("Email")]
        public string PayeeEmail { get; set; }
        [DisplayName("Bank Name")]
        public string PayeeBankName { get; set; }
        [DisplayName("bank Account Number")]
        public string PayeeBankAccount { get; set; }
        [DisplayName("Bank Routing Number")]
        public string PayeeBankRoutingNumber { get; set; }
        public int PayeeTypeID { get; set; }
    }
}
