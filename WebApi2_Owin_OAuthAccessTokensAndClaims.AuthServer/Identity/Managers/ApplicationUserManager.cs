using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Claims;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Contexts;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Services.Communication.Email;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Validators;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers
{
    /// <summary>
    /// Responsible for managing instances of the 'ApplicationUser' class
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        { }

        /// <summary>
        /// Our OWIN middleware will create an instance of'ApplicationUserManager' per request where Identity data is accessed, 
        /// this will help us to hide the details of how IdentityUser is stored throughout the application.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            // Grab the 'IdentityDbContext' instance created by OWIN (see 'Config.OAuth.OAuthAuthorizationServerConfig')
            var dbContext = context.Get<ApplicationDbContext>();

            // Create an instance of AppManager
            var appManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));

            // Configure validation logic for passwords
            appManager.PasswordValidator = new UserPasswordValidator();

            // Configure validation logic for usernames
            appManager.UserValidator = new UserNameValidator(appManager);

            // Configure Claims Generation
            appManager.ClaimsIdentityFactory = new ApplicationClaimsFactory();

            // Configure Email Service
            appManager.EmailService = new GmailSMTPService();

            // Configure email life span for registration confirmations and password resets
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(1)
                };
            }

            return appManager;
        }

    }
}
