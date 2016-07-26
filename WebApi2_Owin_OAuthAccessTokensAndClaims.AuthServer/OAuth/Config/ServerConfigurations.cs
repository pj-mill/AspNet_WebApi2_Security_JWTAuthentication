using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Contexts;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.OAuth.Formats;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.OAuth.Services;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Config
{
    public class OAuthConfiguration
    {
        public static void Register(IAppBuilder app)
        {
            RegisterAuthorizationServer(app);
            RegisterResourceServer(app);
        }

        private static void RegisterAuthorizationServer(IAppBuilder app)
        {
            // Configure the db context, user manager & role manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = false,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                Provider = new OAuthAuthorizationService(),
                AccessTokenFormat = new JWTDataFormat(ConfigurationManager.AppSettings["as:Issuer"].ToString())
            };

#if DEBUG
            // For Dev enviroment only (on production should be AllowInsecureHttp = false)
            OAuthServerOptions.AllowInsecureHttp = true;
#endif

            // OAuth Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        /// <summary>
        /// Cionfigure protection for Resource server resources using JWT
        /// </summary>
        /// <param name="app"></param>
        private static void RegisterResourceServer(IAppBuilder app)
        {
            // Configure our API to trust tokens issued by our Authorization server only, 
            // in our case the Authorization and Resource Server are the same server
            var issuer = ConfigurationManager.AppSettings["as:Issuer"].ToString();
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}
