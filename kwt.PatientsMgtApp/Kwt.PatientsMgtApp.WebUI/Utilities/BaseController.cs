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
            Menu menu = new Menu();
            menu.MenuItem = new List<MenuItems>(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                var menuItem = new MenuItems();
                menuItem.MenuId = (i + 1);

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
                menuItem.ControllerName = items[i];
                menu.MenuItem.Add(menuItem);
            }

            ViewBag.MenuItems = menu;
        }

    }
}