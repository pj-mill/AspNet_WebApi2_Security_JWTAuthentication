using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.ModelFactories;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers.Abstract
{
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Responible for managing the user
        /// </summary>
        private ApplicationUserManager appManager = null;
        protected ApplicationUserManager AppManager
        {
            get
            {
                return appManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        /// <summary>
        /// Responible for managing roles
        /// </summary>
        private ApplicationRoleManager roleManager = null;
        protected ApplicationRoleManager RoleManager
        {
            get
            {
                return roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        /// <summary>
        /// Converts user domain entities to view models and v.v.
        /// </summary>
        private ApplicationUserModelFactory userModelFactory;
        protected ApplicationUserModelFactory UserModelFactory
        {
            get
            {
                if (userModelFactory == null)
                {
                    userModelFactory = new ApplicationUserModelFactory(this.Request, this.AppManager);
                }
                return userModelFactory;
            }
        }

        /// <summary>
        /// Converts role domain entities to view models and v.v.
        /// </summary>
        private RoleModelFactory roleModelFactory;
        protected RoleModelFactory RoleModelFactory
        {
            get
            {
                if (roleModelFactory == null)
                {
                    roleModelFactory = new RoleModelFactory(this.Request);
                }
                return roleModelFactory;
            }
        }


        /// <summary>
        /// Formats the error messages returned to the client
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            return null;
        }
    }
}
