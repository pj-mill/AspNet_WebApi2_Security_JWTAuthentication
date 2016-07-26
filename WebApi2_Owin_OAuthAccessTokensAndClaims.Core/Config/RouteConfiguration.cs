using System.Web.Http;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Config
{
    /// <summary>
    /// Routing Configuration
    /// </summary>
    public class RouteConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}