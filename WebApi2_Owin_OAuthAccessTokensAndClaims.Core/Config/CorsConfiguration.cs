using Owin;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Config
{
    /// <summary>
    /// CORS support configuration
    /// </summary>
    public class CORSConfiguration
    {
        public static void Register(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}