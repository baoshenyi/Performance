using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Performance.Startup))]
namespace Performance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
