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
    }

    public class Menu
    {
        public List<MenuItems> MenuItem { get; set; }
    }
}