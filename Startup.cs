using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carreno_Financial_Portal.Startup))]
namespace Carreno_Financial_Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
