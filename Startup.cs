using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Web.Helpers;
using Microsoft.Owin.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.UI.WebControls;
using Emby.Media.Common.Extensions;

[assembly: OwinStartup(typeof(OnlineBookExchange.Startup))]

namespace OnlineBookExchange
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}