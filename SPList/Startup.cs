using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace SPList
{
    public partial class Startup
    {
        public static string AppName = ConfigurationManager.AppSettings["AppName"];

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
