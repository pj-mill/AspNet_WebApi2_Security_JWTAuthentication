using Microsoft.Owin;
using Owin;
using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Config;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Config;

[assembly: OwinStartup(typeof(WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Startup))]

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            OAuthServerConfigurations.Register(app);

            RouteConfiguration.Register(config);

            JSONMediaTypeConfiguration.Register(config);

            CORSConfiguration.Register(app);

            app.UseWebApi(config);
        }
    }
}
