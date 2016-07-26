using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Validators
{
    public class UserPasswordValidator : PasswordValidator
    {
        // Do not allow the following passwords
        List<string> blackList = new List<string> {
            "111111",
            "123123",
            "1234",
            "12345",
            "123456",
            "1234567",
            "12345678",
            "123456789",
            "1234567890",
            "696969",
            "abc123",
            "baseball",
            "batman",
            "dragon",
            "football",
            "gandalf",
            "hockey",
            "letmein",
            "master",
            "michael",
            "monkey",
            "mustang",
            "password",
            "password1",
            "passw0rd",
            "p@ssword",
            "p@ssw0rd",
            "qwerty",
            "superman",
            "supperman",
            "thomas",
            "welcome",
            "xxxxxx"
        };

        public UserPasswordValidator()
        {
            RequiredLength = 6;
            RequireNonLetterOrDigit = true;
            RequireDigit = false;
            RequireLowercase = true;
            RequireUppercase = true;
        }

        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);

            if (blackList.Contains(password.ToLower()))
            {
                var errors = result.Errors.ToList();
                errors.Add("Password not allowed");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
