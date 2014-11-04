using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Eventor.Startup))]
namespace Eventor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
