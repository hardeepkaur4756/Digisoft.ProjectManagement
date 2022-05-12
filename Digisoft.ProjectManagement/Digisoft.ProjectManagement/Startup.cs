using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Digisoft.ProjectManagement.Startup))]
namespace Digisoft.ProjectManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
