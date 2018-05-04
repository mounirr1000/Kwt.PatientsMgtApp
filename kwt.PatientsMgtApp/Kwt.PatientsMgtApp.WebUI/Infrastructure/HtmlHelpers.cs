using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace Kwt.PatientsMgtApp.WebUI.Infrastructure
{
    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, string id)
        {
            ApplicationUserManager mgr
                = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return new MvcHtmlString(mgr.FindByIdAsync(id).Result.UserName);
        }

        public static MvcHtmlString ListItemAction(this HtmlHelper helper, string name, string actionName, string controllerName)
        {
            var currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            var currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
            var sb = new StringBuilder();
            sb.AppendFormat("<li {0} ", (" class=\"h-item k-item k-state-default k-first\">"));
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            sb.AppendFormat("<a {0} href=\"{1}\">{2}</a>", (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) 
                                                    //&&
                                                    //currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase)
                                                    ? " class=\"k-link k-state-active\"" :
                                                      " class=\"k-link\""),
                                                      url.Action(actionName, controllerName), name);
            sb.Append("</li>");
            return new MvcHtmlString(sb.ToString());
        }

        public static string Id(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            //if (routeValues.ContainsKey("id"))
            //    return (string)routeValues["id"];
            //else 
            //if (HttpContext.Current.Request.QueryString.AllKeys.Contains("id"))
            //    return HttpContext.Current.Request.QueryString["id"];
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                var query = HttpContext.Current.Request.QueryString.AllKeys[0];
                return HttpContext.Current.Request.QueryString[0];
            }
            return string.Empty;
        }
        public static string QueryString(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                var query = HttpContext.Current.Request.QueryString.AllKeys[0];
                return query;
            }
            return string.Empty;
        }

        public static string UrlLink(this HtmlHelper htmlHelper)
        {
            var url = HttpContext.Current.Request.UrlReferrer;
            if (url != null)
            {
                return url.PathAndQuery;
            }
            return string.Empty;
        }
        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string Action(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }
    }
}