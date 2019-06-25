using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kwt.PatientsMgtApp.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Reports",
                url: "KwtReport/{action}",
                defaults: new { controller = "Report", action = "Patient", id = UrlParameter.Optional }
          );
            //routes.MapRoute(
            //    "Reports",                                           // Route name
            //    "KwtReport/List",                            // URL with parameters
            //    new { controller = "Report", action = "Patient" }  // Parameter defaults
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
