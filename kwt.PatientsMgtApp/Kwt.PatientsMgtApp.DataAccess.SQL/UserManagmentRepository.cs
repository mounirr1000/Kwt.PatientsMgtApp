using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class UserManagmentRepository : IUserManagmentRepository
    {

        private readonly IDomainObjectRepository _domainObjectRepository;

        public UserManagmentRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<UserModel> GetUsersList()
        {
            var userList = _domainObjectRepository.All<User>();
            return userList?.Select(u => new UserModel()
            {
                AccessFailedCount = u.AccessFailedCount,
                Email = u.Email,
                UserID = u.UserID,
                UserName = u.UserName,
                PhoneNumber = u.PhoneNumber,
                MiddleName=u.MiddleName,
                FirstName=u.FirstName,
                LastName=u.LastName
                //UserRoles = u.UserRoles.Select(r => new UserRolesModel()
                //{
                //    RoleId = r.RoleId
                //}).ToList()
            }).ToList();

        }

        public List<RoleModel> GetRolesList()
        {
            var userList = _domainObjectRepository.All<Role>();
            return userList?.Select(u => new RoleModel()
            {
                RoleName = u.RoleName,
                Id = u.RoleID,
                
            }).ToList();
        }

        public UserRolesModel GetUserRoles()
        {
            
            return null;
        }
    }
}
