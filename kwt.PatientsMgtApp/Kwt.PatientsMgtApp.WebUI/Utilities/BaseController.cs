using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.WebUI.Models;

namespace Kwt.PatientsMgtApp.WebUI.Utilities
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.Email == username || u.UserName == username);
                    if (user != null)
                    {
                        string fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                        if (fullName.Length > 0)
                        {
                            ViewData.Add("FullName", fullName);
                            Session["FullName"] = fullName;
                        }
                        else
                        {
                            ViewData.Add("FullName", user.Email);
                            Session["FullName"] = user.Email;
                        }
                            
                    }
                    else
                    {
                        ViewData.Add("FullName", username);
                        Session["FullName"] = username;
                    }


                }

            }
            base.OnActionExecuted(filterContext);
        }


        //
        public BaseController()
        {
            IntialiseMenu();
        }
        public void Success(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Success, message, dismissable);
        }

        public void Information(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Information, message, dismissable);
        }

        public void Warning(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Warning, message, dismissable);
        }

        public void Danger(string message, bool dismissable = false)
        {
            AddAlert(AlertStyles.Danger, message, dismissable);
        }

        private void AddAlert(string alertStyle, string message, bool dismissable)
        {
            var alerts = TempData.ContainsKey(Alert.TempDataKey)
                ? (List<Alert>)TempData[Alert.TempDataKey]
                : new List<Alert>();

            alerts.Add(new Alert
            {
                AlertStyle = alertStyle,
                Message = message,
                Dismissable = dismissable
            });

            TempData[Alert.TempDataKey] = alerts;
        }

        private void IntialiseMenu()
        {
            string[] items = new[] { "Home", "Patient", "Companion", "Payment", "Report", "Admin" };
            string[] subitems = new[] { "Patient", "Employee", "Others", "Payrolls" };
            string[] subPayitems = new[] { "Regular", "Bonus", "Overtime", "Severance" };
            string[] subitemsActionName = new[] { "List", "EmployeePayments", "OthersPayments", "Payrolls" };
            string[] subPayitemsActionName = new[] { "EmployeeRegular", "EmployeeBonus", "EmployeeOvertime", "EmployeeSeverance" };
            string[] subMenuIcons = new[] { "submenupatient", "submenuworker", "submenucheck", "submenupayroll" };
            string[] subMenuPayIcons = new[] { "regularPaySubmenu", "bonusPaySubmenu", "overtimePaySubmenu", "serverancePaySubmenu" };
            string[] icons = new[] { "houseMenu", "patientMenu", "support", "cashMenu", "planningMenu", "adminMenu" };
            string[] colors = new[] { "coral", "", "", "yellowgreen", "deeppink", "grey" };
            TopMenu menu = new TopMenu();
            menu.MenuItem = new List<MenuItems>(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                var menuItem = new MenuItems();
                menuItem.MenuId = (i + 1);
                menuItem.IconName = icons[i];
                menuItem.Color = colors[i];
                if (items[i] == "Home")
                {
                    menuItem.MenuName = items[i];
                    menuItem.ActionName = "Index";
                }
                else
                {
                    menuItem.MenuName = items[i] + "s";
                    menuItem.ActionName = "List";
                }
                //new 
                if (items[i] == "Payment")
                {

                    SubMenu subMenu = new SubMenu();
                    subMenu.MenuItem = new List<MenuItems>(subitems.Length);
                    //for (int j = 0; j < subitems.Length; j++)
                    //{
                    //    var subMenuItem = new MenuItems();
                    //    subMenuItem.MenuId = (j + 1);
                    //    subMenuItem.IconName = subMenuIcons[j];
                    //    subMenuItem.Color = colors[j];
                    //    subMenuItem.ControllerName = items[i];// that is payment controller
                    //    subMenuItem.MenuName = subitems[j] + " Payments";
                    //    subMenuItem.ActionName = subitemsActionName[j];
                    //    subMenu.MenuItem.Add(subMenuItem);

                    //}
                    for (int j = 0; j < subitems.Length; j++)
                    {
                        var subMenuItem = new MenuItems();
                        if (subitems[j] == "Employee")
                        {
                            SubMenu subPayMenu = new SubMenu();
                            subPayMenu.MenuItem = new List<MenuItems>(subPayitems.Length);
                            for (int m = 0; m < subPayitems.Length; m++)
                            {
                                var subPayMenuItem = new MenuItems();
                                subPayMenuItem.MenuId = (m + 1);
                                subPayMenuItem.IconName = subMenuPayIcons[m];
                                //subPayMenuItem.Color = colors[m];
                                subPayMenuItem.ControllerName = items[i];// that is payment controller
                                subPayMenuItem.MenuName = subPayitems[m] + " Pay";
                                subPayMenuItem.ActionName = subPayitemsActionName[m];
                                subPayMenu.MenuItem.Add(subPayMenuItem);
                            }
                            subMenuItem.SubMenu = subPayMenu;
                            //subPayMenu.MenuItem;
                        }

                        subMenuItem.MenuId = (j + 1);
                        subMenuItem.IconName = subMenuIcons[j];
                        subMenuItem.Color = colors[j];
                        subMenuItem.ControllerName = items[i];// that is payment controller
                        //subMenuItem.MenuName = subitems[j] != "Payrolls" ? subitems[j] + " Payments" : subitems[j];
                        subMenuItem.MenuName = subitems[j] != "Payrolls" ? (subitems[j] != "Employee" ? subitems[j] + " Payments": subitems[j] + "s") : subitems[j];
                        subMenuItem.ActionName = subitemsActionName[j];
                        subMenu.MenuItem.Add(subMenuItem);

                    }

                    menuItem.SubMenu = subMenu;
                }
                //
                menuItem.ControllerName = items[i];
                menu.MenuItem.Add(menuItem);
            }

            ViewBag.MenuItems = menu;
        }

    }
}