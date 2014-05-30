using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RoslynMapper.WebSite.Startup))]
namespace RoslynMapper.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
