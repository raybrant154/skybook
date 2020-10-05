using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Skybook.Startup))]
namespace Skybook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
