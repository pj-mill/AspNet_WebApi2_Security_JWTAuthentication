using Microsoft.AspNet.Identity;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Validators
{
    public class UsernameValidator : UserValidator<ApplicationUser>
    {
        public UsernameValidator(UserManager<ApplicationUser, string> manager) : base(manager)
        {
        }
    }
}
