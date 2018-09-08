using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AksonApp.Startup))]
namespace AksonApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
