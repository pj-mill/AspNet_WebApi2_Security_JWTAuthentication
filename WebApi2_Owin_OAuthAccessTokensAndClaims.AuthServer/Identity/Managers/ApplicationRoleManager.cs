using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Contexts;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers
{
    /// <summary>
    /// Responsible for managing instances of the 'Role' class
    /// </summary>
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }

        /// <summary>
        /// Our OWIN middleware will create an instance of'ApplicationRoleManager' per request where Identity data is accessed, 
        /// this will help us to hide the details of how IdentityUser is stored throughout the application.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            // Grab the 'IdentityDbContext' instance created by OWIN (see 'Config.OAuth.OAuthAuthorizationServerConfig')
            var dbContext = context.Get<ApplicationDbContext>();

            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(dbContext));

            return appRoleManager;
        }
    }
}
