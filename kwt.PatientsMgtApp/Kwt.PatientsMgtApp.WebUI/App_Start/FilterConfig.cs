using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;

namespace Kwt.PatientsMgtApp.WebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
