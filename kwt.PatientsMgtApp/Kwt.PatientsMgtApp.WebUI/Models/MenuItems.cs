using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class MenuItems
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string IconName { get; set; }
        public string Color { get; set; }

        public SubMenu SubMenu { get; set; }
    }

    public class TopMenu
    {
        public List<MenuItems> MenuItem { get; set; }
    }

    //==================================
    public class SubMenu
    {
        public List<MenuItems> MenuItem { get; set; }
    }
}