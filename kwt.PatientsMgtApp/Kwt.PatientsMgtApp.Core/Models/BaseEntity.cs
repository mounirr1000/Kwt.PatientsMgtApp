using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Interfaces;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BaseEntity : IDomainObject
    {
        public int Id { get; set; }
    }
}
