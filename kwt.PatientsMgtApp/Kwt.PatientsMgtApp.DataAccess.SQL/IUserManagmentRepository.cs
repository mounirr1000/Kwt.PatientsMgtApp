﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IUserManagmentRepository
    {
        List<UserModel> GetUsersList();
        List<RoleModel> GetRolesList();

        UserRolesModel GetUserRoles();
    }
}
