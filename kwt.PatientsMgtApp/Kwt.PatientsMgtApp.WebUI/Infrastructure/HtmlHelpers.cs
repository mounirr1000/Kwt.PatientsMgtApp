using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.WebUI.Models;
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

        //public static MvcHtmlString ListItemAction(this HtmlHelper helper, string name, string actionName, string controllerName, string iconName, string color)
        public static MvcHtmlString ListItemAction(this HtmlHelper helper, string name, string actionName, string controllerName, string iconName, string color, SubMenu subMenu = null)
        {
            var currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            var currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
            var sb = new StringBuilder();
            sb.AppendFormat("<li {0} ", (" class=\"h-item k-item k-state-default k-first dropdown\">"));
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

            //sb.AppendFormat("<a {0} href=\"{1}\"><i class=\"fa fa-{3}\" style=\"color:{4}\"></i> {2}</a>", (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
            sb.AppendFormat("<a {0} href=\"{1}\" id=\"{4}\"><div class=\"{3}\" style=\"float:left\"></div>&nbsp {2}</a>", (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
                                                    //&&
                                                    //currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase)
                                                    ? " class=\"k-link k-state-active\"" :
                                                      " class=\"k-link\""),
                                                      url.Action(actionName, controllerName), name, iconName, controllerName);

            //new 8-3-2019

            //if (name == "Payments" && subMenu != null)
            //{
            //    sb.AppendFormat("<div   class=\"dropdown\" style=\"display:inline;\">");
            //    sb.AppendFormat("<div {0} ", ("class=\"dropdown-content\">"));
            //    foreach (var menu in subMenu.MenuItem)
            //    {


            //        sb.AppendFormat("<a {0} href=\"{1}\">&nbsp {2}</a>", (currentControllerName.Equals(menu.ControllerName, StringComparison.CurrentCultureIgnoreCase)
            //                                            //&&
            //                                            //currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase)
            //                                            ? " class=\"k-link k-state-active\"" :
            //                                              " class=\"k-link\""),
            //                                              url.Action(menu.ActionName, menu.ControllerName), menu.MenuName, menu.IconName);

            //    }
            //    sb.Append("</div>");
            //    sb.AppendFormat("</div>");
            //}
            if (name == "Payments" && subMenu != null)
            {
                sb.AppendFormat("<div   class=\"dropdown\" style=\"display:inline;\">");
                sb.AppendFormat("<div {0} ", ("class=\"dropdown-content\">"));
                foreach (var menu in subMenu.MenuItem)
                {


                    //sb.AppendFormat("<a {0} href=\"{1}\"><div class=\"{3}\" style=\"float:left\"></div>&nbsp {2}</a>", (currentControllerName.Equals(menu.ControllerName, StringComparison.CurrentCultureIgnoreCase)
                    //                                    &&
                    //                                    currentActionName.Equals(menu.ActionName, StringComparison.CurrentCultureIgnoreCase)
                    //                                    ? " class=\"k-link k-state-active\"" :
                    //                                      " class=\"dropdownlink\""),
                    //                                      url.Action(menu.ActionName, menu.ControllerName), menu.MenuName, menu.IconName);



                   // if (menu.MenuName == "Employee Payments")
                    if (menu.MenuName == "Employees")
                    {
                        sb.AppendFormat("<div {0} ", ("class=\"dropdown-submenu\">"));
                        sb.AppendFormat("<a {0} href=\"{1}\" id=\"employeePay\"><div class=\"{3}\" style=\"float:left\" ></div>&nbsp {2} <span class=\"caret\"></span></a>", (currentControllerName.Equals(menu.ControllerName, StringComparison.CurrentCultureIgnoreCase)
                                                            &&
                                                            currentActionName.Equals(menu.ActionName, StringComparison.CurrentCultureIgnoreCase)
                                                            ? " class=\"k-link k-state-active\"" :
                                                              " class=\"dropdownlink\""),
                                                              url.Action(menu.ActionName, menu.ControllerName), menu.MenuName, menu.IconName);
                        sb.AppendFormat("<div   class=\"dropdown-menu\" id=\"paySubmenu\">");

                        foreach (var menuPay in menu?.SubMenu?.MenuItem)
                        {
                            // sb.AppendFormat("<div {0} ", ("class=\"\">"));
                            sb.AppendFormat("<a {0} href=\"{1}\"><div class=\"{3}\" style=\"float:left\"></div>&nbsp {2}</a>", (currentControllerName.Equals(menuPay.ControllerName, StringComparison.CurrentCultureIgnoreCase)
                                                            &&
                                                            currentActionName.Equals(menuPay.ActionName, StringComparison.CurrentCultureIgnoreCase)
                                                            ? " class=\"k-link k-state-active\"" :
                                                              " class=\"dropdownlink\""),
                                                              url.Action(menuPay.ActionName, menuPay.ControllerName), menuPay.MenuName, menuPay.IconName);
                            //   sb.Append("</div>");
                        }

                        sb.AppendFormat("</div>");

                        sb.AppendFormat("</div>");
                    }
                    else
                    {
                        sb.AppendFormat("<a {0} href=\"{1}\"><div class=\"{3}\" style=\"float:left\"></div>&nbsp {2}</a>", (currentControllerName.Equals(menu.ControllerName, StringComparison.CurrentCultureIgnoreCase)
                                                            &&
                                                            currentActionName.Equals(menu.ActionName, StringComparison.CurrentCultureIgnoreCase)
                                                            ? " class=\"k-link k-state-active\"" :
                                                              " class=\"dropdownlink\""),
                                                              url.Action(menu.ActionName, menu.ControllerName), menu.MenuName, menu.IconName);
                    }

                }
                sb.Append("</div>");
                sb.AppendFormat("</div>");

            }
            // end new
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
        public static IDisposable BeginCollectionItem<TModel>(this HtmlHelper<TModel> html, string collectionName)
        {
            //string itemIndex = Guid.NewGuid().ToString();
            string itemIndex = GetCollectionItemIndex(collectionName);
            string collectionItemName = String.Format("{0}[{1}]", collectionName, itemIndex);

            TagBuilder indexField = new TagBuilder("input");
                 indexField.MergeAttributes(new Dictionary<string, string>() {
                { "name", String.Format("{0}.Index", collectionName) },
                { "value", itemIndex },
                { "type", "hidden" },
                { "autocomplete", "off" }
            });

            html.ViewContext.Writer.WriteLine(indexField.ToString(TagRenderMode.SelfClosing));
            return new CollectionItemNamePrefixScope(html.ViewData.TemplateInfo, collectionItemName);
        }
        private static string GetCollectionItemIndex(string collectionIndexFieldName)
        {
            Queue<string> previousIndices = (Queue<string>)HttpContext.Current.Items[collectionIndexFieldName];
            if (previousIndices == null)
            {
                HttpContext.Current.Items[collectionIndexFieldName] = previousIndices = new Queue<string>();

                string previousIndicesValues = HttpContext.Current.Request[collectionIndexFieldName];
                if (!String.IsNullOrWhiteSpace(previousIndicesValues))
                {
                    foreach (string index in previousIndicesValues.Split(','))
                        previousIndices.Enqueue(index);
                }
            }

            return previousIndices.Count > 0 ? previousIndices.Dequeue() : Guid.NewGuid().ToString();
        }
        private class CollectionItemNamePrefixScope : IDisposable
        {
            private readonly TemplateInfo _templateInfo;
            private readonly string _previousPrefix;

            public CollectionItemNamePrefixScope(TemplateInfo templateInfo, string collectionItemName)
            {
                this._templateInfo = templateInfo;

                _previousPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = collectionItemName;
            }

            public void Dispose()
            {
                _templateInfo.HtmlFieldPrefix = _previousPrefix;
            }
        }
    }
}