using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Person Person { get; set; }
    }
}
