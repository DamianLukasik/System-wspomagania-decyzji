using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrystalSiege.Startup))]
namespace CrystalSiege
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
