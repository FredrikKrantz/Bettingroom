using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BettingRoom.Startup))]
namespace BettingRoom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
