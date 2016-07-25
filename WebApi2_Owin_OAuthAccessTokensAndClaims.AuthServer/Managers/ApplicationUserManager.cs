using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.DataAccessLayer.Contexts;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Managers
{
    /// <summary>
    /// Responsible for managing instances of the 'ApplicationUser' class
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        /// <summary>
        /// Our OWIN middleware will create an instance of'ApplicationUserManager' per request
        /// Uses the Owin middleware to create instances of 'ApplicationUserManager' for each request where
        /// Identity data is accessed, this will help us to hide the details of how IdentityUser is stored 
        /// throughout the application.
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




            return appManager;
        }

    }
}
