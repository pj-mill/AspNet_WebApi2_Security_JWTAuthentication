using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Person Person { get; set; }
    }
}
