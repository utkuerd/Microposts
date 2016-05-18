using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Microposts.Startup))]
namespace Microposts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
