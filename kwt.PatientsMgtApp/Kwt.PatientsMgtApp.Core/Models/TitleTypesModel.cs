using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class TitleTypesModel
    {
        public int TitleTypeId { get; set; }
        public string TitleTypeName { get; set; }
        public Nullable<decimal> TitleYearlyIncrease { get; set; }

    }
}
