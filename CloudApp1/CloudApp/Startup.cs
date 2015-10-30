using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudApp.Startup))]
namespace CloudApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
