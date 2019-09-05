using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kwt.PatientsMgtApp.WebUI.Controllers;

namespace Kwt.PatientsMgtApp.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            HttpException httpex = ex as HttpException;
            //Server.Transfer("~/DefaultErrorPage.aspx");
            RouteData data = new RouteData();
            data.Values.Add("Controller","Error");

            if (httpex == null)
            {
                data.Values.Add("action", "Index");
            }
            else
            {
                switch (httpex.GetHttpCode())
                {
                    case 404:
                        data.Values.Add("action", "Status404");
                        break;
                    case 405:
                        data.Values.Add("action", "Status405");
                        break;
                    default:
                        data.Values.Add("action", "GeneralError");
                        break;
                }
            }
            Server.ClearError();
            Response.TrySkipIisCustomErrors = true;
            IController error= new ErrorController();
            error.Execute(new RequestContext(new HttpContextWrapper(Context),data ));
        }
    }
}
