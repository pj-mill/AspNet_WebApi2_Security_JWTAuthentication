using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.OAuth.Services
{
    public class OAuthAuthorizationService : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // For this exercise, we are considering the request valid always because in our implementation,
            // our client is a trusted client and we do not need to validate it.
            // See more at: http://stackoverflow.com/questions/24340088/owin-web-api-2-adding-additional-logic-to-bearer-authorization
            //              http://www.hackered.co.uk/articles/asp-net-mvc-creating-an-oauth-client-credentials-grant-type-token-endpoint

            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Responsible for receiving the username and password from the request and validate them against our ASP.NET Identity system
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // For this exercise we will allow all domain names access
            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var appManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            // Authenticate user
            ApplicationUser user = await appManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError("invalid_grant", "User did not confirm email.");
                return;
            }

            // Get identity for user
            ClaimsIdentity oAuthIdentity = await appManager.CreateIdentityAsync(user, "JWT"); //await user.GenerateUserIdentityAsync(userManager, "JWT");

            // Add a new claim determining if the user is a full time employee or not (This could be achieved in our Custom Claims Factory)
            //oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));

            // Add a new claim determining if the user can make refunds or not (must be a full time employee)
            //oAuthIdentity.AddClaims(IncidentResolverClaim.CheckClaim(oAuthIdentity));

            // An Authentication ticket which contains the identity for the authenticated user
            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            // Transfer the identity to an OAuth 2.0 bearer access token
            context.Validated(ticket);

        }

    }
}
