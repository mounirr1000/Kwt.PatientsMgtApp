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
            string[] subitems = new[] { "Patient", "Folder2", "Folder3", "Folder4", "Folder5", "Folder6" };
            string[] subitemsActionName = new[] { "Index", "Folder2", "Folder3", "Folder4", "Folder5", "Folder6" };
            //string[] icons = new[] { "home", "address-book", "user-friends", "dollar", "file-text", "cogs" };
            string[] icons = new[] { "houseMenu", "patientMenu", "support", "cashMenu", "planningMenu", "adminMenu" };
            string[] colors = new[] { "coral", "", "", "yellowgreen", "deeppink", "grey" };
            Menu menu = new Menu();
            menu.MenuItem = new List<MenuItems>(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                var menuItem = new MenuItems();
                menuItem.MenuId = (i + 1);
                menuItem.IconName= icons[i];
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
                if (items[i] == "Patient")
                {
                    
                    SubMenu  subMenu = new SubMenu();
                    subMenu.MenuItem = new List<MenuItems>(subitems.Length);
                    for (int j = 0; j < subitems.Length; j++)
                    {
                        var subMenuItem = new MenuItems();
                        subMenuItem.MenuId = (j + 1);
                        subMenuItem.IconName = icons[j];
                        subMenuItem.Color = colors[j];
                        subMenuItem.ControllerName = subitems[0];
                        subMenuItem.MenuName = subitems[j] + "s";
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