using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(museumProj.Startup))]
namespace museumProj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
