﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kwt.PatientsMgtApp.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Kwt.PatientsMgtApp.WebUI.Infrastructure
{
    public class AppRoleManager : RoleManager<IdentityRole>, IDisposable
    {

        public AppRoleManager(RoleStore<IdentityRole> store)
            : base(store)
        {
        }

        public static AppRoleManager Create(
                IdentityFactoryOptions<AppRoleManager> options,
                IOwinContext context)
        {
            return new AppRoleManager(new
                RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
}