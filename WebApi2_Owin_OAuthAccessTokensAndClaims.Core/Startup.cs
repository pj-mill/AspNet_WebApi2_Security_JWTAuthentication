using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Startup))]

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();


        }
    }
}
