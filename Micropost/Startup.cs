using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Micropost.Startup))]
namespace Micropost
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
