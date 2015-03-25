using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NoSqlStore.Startup))]
namespace NoSqlStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
