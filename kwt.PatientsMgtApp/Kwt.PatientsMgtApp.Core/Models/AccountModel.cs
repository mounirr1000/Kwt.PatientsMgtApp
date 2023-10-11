using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class AccountModel
    {
        public int AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Descriptions { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string AmountDecimalFormat { get { return Amount!=null? Amount?.ToString("0.##"):"0"; } }
        public string AmountFormated { get { return Amount != null ? Amount?.ToString("C2") : ""; } }
        public Nullable<decimal> Discount { get; set; }
        public string DiscountDecimalFormat { get { return FormatDiscount(Discount, DiscountTypeId); } }
        public Nullable<int> PayrollAccountID { get; set; }
        public Nullable<int> PayrollID { get; set; }
        public PayrollAccountModel PayrollAccount { get; set; }
        public List<PayrollAccountModel> PayrollAccounts { get; set; }

        public decimal? TotalAmount { get; set; }

        public string TotalAmountFormated { get { return TotalAmount!=null? TotalAmount?.ToString("C2"):""; } }

        public List<AccountModel> Accounts { get; set; }
        public Nullable<int> DiscountTypeId { get; set; }
        public Nullable<decimal> AmountAfterDiscount { get; set; }
        public  DiscountTypeModel DiscountType { get; set; }

        public string DiscountTypeName { get; set; }
        public  List<DiscountTypeModel> DiscountTypes { get; set; }

        private string FormatDiscount(decimal? discount, int? discountTypeId)
        {
            string fomart = "0";
            if (discountTypeId == 1)//percentage
                fomart = discount?.ToString("0.##") + "%";
            if (discountTypeId == 2)// dollar value
                fomart = discount?.ToString("C2");

            return fomart;
        }
    }
}
