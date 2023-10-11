using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class TaxCategoryModel
    {
        public int TaxCategoryID { get; set; }
        public string TaxCategory1 { get; set; }
        public Nullable<decimal> TaxCategoryValue { get; set; }
    }
}
