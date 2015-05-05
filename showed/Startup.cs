using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(showed.Startup))]
namespace showed
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
