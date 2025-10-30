using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinanceTrackerProject.Startup))]
namespace FinanceTrackerProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
