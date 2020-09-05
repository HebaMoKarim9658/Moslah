using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoslahConsumer.Startup))]
namespace MoslahConsumer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
