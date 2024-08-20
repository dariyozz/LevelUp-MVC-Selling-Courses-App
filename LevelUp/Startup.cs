using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LevelUp.Startup))]
namespace LevelUp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
