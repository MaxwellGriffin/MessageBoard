using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessageBoard_2.WebMVC.Startup))]
namespace MessageBoard_2.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
