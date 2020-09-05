using Microsoft.Owin;
using Moslah.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(Moslah.Startup))]
namespace Moslah
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
