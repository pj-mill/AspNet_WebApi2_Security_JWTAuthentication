using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Validators
{
    /// <summary>
    /// Responsible for validating user name and email
    /// </summary>
    public class UserNameValidator : UserValidator<ApplicationUser>
    {
        // Restrict domain names to the following
        List<string> whiteList = new List<string> { "outlook.ie", "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };

        public UserNameValidator(ApplicationUserManager manager) : base(manager)
        {
            AllowOnlyAlphanumericUserNames = true;
            RequireUniqueEmail = true;
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            var result = await base.ValidateAsync(user);

            // Validate emails (only allow whitelist)
            var emailDomain = user.Email.Split('@')[1];

            if (!whiteList.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
