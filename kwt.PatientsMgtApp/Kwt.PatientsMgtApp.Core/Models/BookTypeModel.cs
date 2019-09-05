using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BookTypeModel:BaseEntity
    {
        public int BookTypeID { get; set; }
        public string BookType { get; set; }
    }
}
