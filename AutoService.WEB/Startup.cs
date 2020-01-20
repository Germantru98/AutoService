using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoService.WEB.Startup))]
namespace AutoService.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
