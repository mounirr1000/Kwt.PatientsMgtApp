using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kwt.PatientsMgtApp.Core;

namespace Kwt.PatientsMgtApp.WebUI.Infrastructure
{
    public class CrudRoles
    {
        public static string[] PatientCrudRoles => new []{Roles.Accountant,Roles.Admin,Roles.Auditor,Roles.Editor,Roles.SuperAdmin,Roles.Medical, Roles.User};
        public  const  string PatientCrudRolesForAutorizeAttribute =  Roles.Accountant+", "+ Roles.Admin+ ", " + Roles.Auditor+ ", " + Roles.Editor+ ", " + Roles.SuperAdmin+ ", " + Roles.Medical+ ", " + Roles.User;
        public static string[] PatientCreateRoles => new []{Roles.Admin,Roles.Editor,Roles.SuperAdmin};
        public const string PatientCreateRolesForAutorizeAttribute =  Roles.Admin + ", "  + Roles.Editor + ", " + Roles.SuperAdmin;
        public static string[] PatientUpdateRoles => new[] { Roles.Admin, Roles.Editor, Roles.SuperAdmin, Roles.Medical };
        public const string PatientUpdateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical;
        public static string[] PatientReadRoles => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public const string PatientReadRolesForAutorizeAttribute = Roles.Accountant + ", " + Roles.Admin + ", " + Roles.Auditor + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical + ", " + Roles.User;
        public static string[] PatientDeleteRoles => new[] { Roles.Admin, Roles.Editor, Roles.SuperAdmin};
        public const string PatientDeleteRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Editor + ", " + Roles.SuperAdmin ;

        //companion
        public static string[] CompanionCrudRoles => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public const string CompanionCrudRolesForAutorizeAttribute = Roles.Accountant + ", " + Roles.Admin + ", " + Roles.Auditor + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical + ", " + Roles.User;
        public static string[] CompanionCreateRoles => new[] { Roles.Admin, Roles.Editor, Roles.SuperAdmin };
        public const string CompanionCreateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Editor + ", " + Roles.SuperAdmin;
        public static string[] CompanionUpdateRoles => new[] {Roles.Admin, Roles.Editor, Roles.SuperAdmin};
        public const string CompanionUpdateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Editor + ", " + Roles.SuperAdmin;
        public static string[] CompanionReadRoles => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public const string CompanionReadRolesForAutorizeAttribute = Roles.Accountant + ", " + Roles.Admin + ", " + Roles.Auditor + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical + ", " + Roles.User;
        public static string[] CompanionDeleteRoles => new[] { Roles.Admin, Roles.Editor, Roles.SuperAdmin };
        public const string CompanionDeleteRolesForAutorizeAttribute =  Roles.Admin + ", " + Roles.Editor + ", " + Roles.SuperAdmin ;

        //payment
        public static string[] PaymentCrudRoles => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public const string PaymentCrudRolesForAutorizeAttribute = Roles.Accountant + ", " + Roles.Admin + ", " + Roles.Auditor + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical + ", " + Roles.User;
        public static string[] PaymentCreateRoles => new[] { Roles.Admin, Roles.Accountant, Roles.SuperAdmin };
        public const string PaymentCreateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Accountant + ", " + Roles.SuperAdmin ;
        public static string[] PaymentUpdateRoles => new[] { Roles.Admin, Roles.Accountant, Roles.SuperAdmin };
        public const string PaymentUpdateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Accountant + ", " + Roles.SuperAdmin;
        public static string[] PaymentReadRoles => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public const string PaymentReadRolesForAutorizeAttribute = Roles.Accountant + ", " + Roles.Admin + ", " + Roles.Auditor + ", " + Roles.Editor + ", " + Roles.SuperAdmin + ", " + Roles.Medical + ", " + Roles.User;
        public static string[] PaymentDeleteRoles => new[] { Roles.Admin, Roles.Accountant, Roles.SuperAdmin };
        public const string PaymentDeleteRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.Accountant + ", " + Roles.SuperAdmin;

        //admin
        public static string[] AdminCrudRoles => new[] { Roles.Admin, Roles.SuperAdmin };
        public const string AdminCrudRolesForAutorizeAttribute = Roles.Admin + ", "  + Roles.SuperAdmin ;
        public static string[] AdminCreateRoles => new[] {Roles.SuperAdmin };
        public const string AdminCreateRolesForAutorizeAttribute =  Roles.SuperAdmin;
        public static string[] AdminUpdateRoles => new[] { Roles.Admin, Roles.SuperAdmin };
        public const string AdminUpdateRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.SuperAdmin;
        public static string[] AdminReadRoles => new[] { Roles.Admin, Roles.SuperAdmin };
        public const string AdminReadRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.SuperAdmin;
        public static string[] AdminDeleteRoles => new[] { Roles.Admin, Roles.SuperAdmin };
        public const string AdminDeleteRolesForAutorizeAttribute = Roles.Admin + ", " + Roles.SuperAdmin;

        public static string[] MainMenuAccess => new[] { Roles.Accountant, Roles.Admin, Roles.Auditor, Roles.Editor, Roles.SuperAdmin, Roles.Medical, Roles.User };
        public static string[] MainMenuAdminAccess => new[] {  Roles.Admin,  Roles.SuperAdmin};

    }

    
}