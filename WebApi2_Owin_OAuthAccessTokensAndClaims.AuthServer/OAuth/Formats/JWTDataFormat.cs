using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.OAuth.Formats
{
    /// <summary>
    /// Responsible for formatting our JWT Token.
    /// By doing this, the requester for an OAuth 2.0 access token from our API will receive a signed token which contains 
    /// claims for an authenticated Resource Owner (User) and this access token is intended only for a certain (Audience) as well.
    /// </summary>
    public class JWTDataFormatConfiguration : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string issuer = string.Empty;

        public JWTDataFormatConfiguration(string issuer)
        {
            this.issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // this API serves as Resource and Authorization Server at the same time, 
            // so we are fixing the Audience Id and Audience Secret (Resource Server) in web.config file, 
            // this Audience Id and Secret will be used for HMAC265 and hash the JWT token. (see: AudienceService for generating these)
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            // prepare the raw data for the JSON Web Token which will be issued to the requester by providing the 
            // issuer, audience, user claims, issue date, expiry date, and the signing key which will sign (hash) the JWT payload.
            var token = new JwtSecurityToken(issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            // Serialize the JSON Web Token to a string and return it to the requester.
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
