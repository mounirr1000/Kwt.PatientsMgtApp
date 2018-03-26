using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kwt.PatientsMgtApp.WebUI.Startup))]
namespace Kwt.PatientsMgtApp.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
