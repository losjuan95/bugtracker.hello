using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bugtracker.Startup))]
namespace bugtracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
