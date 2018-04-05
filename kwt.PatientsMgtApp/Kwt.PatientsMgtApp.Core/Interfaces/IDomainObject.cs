using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Interfaces
{
    public interface IDomainObject
    {
        int Id { get; set; }

      
    }

    public interface IAuditObject
    {
        DateTime CreatedDate { get;set;}
        DateTime? ModifiedDate { get; set; }
        //DateTime? DeletedDate { get; set; }
    }
}
