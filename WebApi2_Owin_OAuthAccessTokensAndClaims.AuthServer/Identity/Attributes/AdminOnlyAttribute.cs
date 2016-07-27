using System;
using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Helpers;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Attributes
{
    /// <summary>
    /// Authorization attribute that allows only administrator access
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            var authorizedRoles = new[] { RoleNames.Admin, RoleNames.SuperAdmin };
            Roles = string.Join(",", authorizedRoles);
        }
    }
}
