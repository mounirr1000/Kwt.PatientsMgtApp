using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kwt.PatientsMgtApp.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class UserViewModel
    {
        public List<UserModel> Users { get; set; }

        public UserModel User { get; set; }

        public List<RoleModel> Roles { get; set; }

        public List<string> UserRoles { get; set; }

    }
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }

    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}