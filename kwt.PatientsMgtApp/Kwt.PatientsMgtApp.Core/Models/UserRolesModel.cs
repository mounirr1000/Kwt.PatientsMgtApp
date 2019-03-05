using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class UserRolesModel: BaseEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual RoleModel Role { get; set; }
        public virtual UserModel User { get; set; }
    }
}
