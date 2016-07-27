using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Attributes;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Claims;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers.Abstract;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers
{
    [AdminOnly]
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        #region APPLICATION USERS
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppManager.Users.ToList().Select(u => this.UserModelFactory.Create(u)));
        }

        /*
       
        THIS IS PERFECTLY GOOD CODE, BUT IS USED ONLY FOR AN ADMIN.
        IT WAS SIMPLY COMMENTED SO AS TO USE THE ACTION BELOW THIS WHICH CAN BE USED BY A NON ADMIN.

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);
            if (user != null)
            {
                return Ok(this.UserModelFactory.Create(user));
            }
            return NotFound();
        }
        */


        /// <summary>
        /// Gets a user by their id
        /// This action is slightly different from the above in that it ...
        /// (A) Does not check for the 'Admin' role. This would be used in such cases as when a user wants to view their own data.
        /// (B) Ensure that the user views their data and not someone else's. Toachieve this we add an extra bit of granularity when validating.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            // Check that it's the same user by comparing id's
            if (User.Identity.GetUserId() != Id)
            {
                return BadRequest("You are not permitted to view this data");
            }
            var user = await this.AppManager.FindByIdAsync(Id);
            if (user != null)
            {
                return Ok(this.UserModelFactory.Create(user));
            }
            return NotFound();
        }

        /// <summary>
        /// Gets a user by name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppManager.FindByNameAsync(username);
            if (user != null)
            {
                return Ok(this.UserModelFactory.Create(user));
            }
            return NotFound();
        }

        /// <summary>
        /// Allows a user to register their credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> register(ApplicationUserRequestModel model)
        {
            /*----------------------------------------------------------------------------
                Validate request model
            ----------------------------------------------------------------------------*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*----------------------------------------------------------------------------
                Check if user already exists
            ----------------------------------------------------------------------------*/
            var existingUser = await this.AppManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            /*----------------------------------------------------------------------------
                Check if role already exists
            ----------------------------------------------------------------------------*/
            var role = await this.RoleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return BadRequest("Invalid role");
            }

            /*----------------------------------------------------------------------------
                Create user
            ----------------------------------------------------------------------------*/
            var user = UserModelFactory.Create(model);
            user.EmailConfirmed = true; // For this exercise, we will auto confirm user (comment out if you want to use email confirmation)

            IdentityResult addUserResult = await this.AppManager.CreateAsync(user, model.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            /*----------------------------------------------------------------------------
                Add user to role
            ----------------------------------------------------------------------------*/
            await this.AppManager.AddToRoleAsync(user.Id, role.Name);

            /*----------------------------------------------------------------------------
                Send confirmation email
            ----------------------------------------------------------------------------*/
            /*
             * // UNCOMMENT THIS IF YOU WANT TO FORCE USER TO CONFIRM THEIR EMAIL
            string code = await this.AppManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            await this.AppManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            */
            /*----------------------------------------------------------------------------
                Return 'OK' with user uri
            ----------------------------------------------------------------------------*/
            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
            return Created(locationHeader, UserModelFactory.Create(user));
        }

        /// <summary>
        ///  Deletes a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var appUser = await this.AppManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }
            return NotFound();
        }

        /// <summary>
        /// Allows a user to confirm their email address
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            // Confirm email address
            IdentityResult result = await this.AppManager.ConfirmEmailAsync(userId, code);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Changes a users password (allows anyone who is authorised to do this)
        /// To add another level of validating granularity, maybe check the current password first ???
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(PasswordChangeRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }
        #endregion

        #region ROLES
        /// <summary>
        /// Manages all roles for a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolesToAssign"></param>
        /// <returns></returns>
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppManager.GetRolesAsync(appUser.Id);

            // Get any roles that do not already exist
            var rolesNotExists = rolesToAssign.Except(this.RoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Any())
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exist in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            // Remove all roles for user
            IdentityResult removeResult = await this.AppManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }
            return Ok();
        }
        #endregion

        #region CLAIMS

        /// <summary>
        /// Manages claims for a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimsToAssign"></param>
        /// <returns></returns>
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimsRequestModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimsRequestModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppManager.RemoveClaimAsync(id, ApplicationClaimsFactory.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppManager.AddClaimAsync(id, ApplicationClaimsFactory.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        /// <summary>
        /// Removes claims for a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimsToRemove"></param>
        /// <returns></returns>
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimsRequestModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimsRequestModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppManager.RemoveClaimAsync(id, ApplicationClaimsFactory.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

        #endregion
    }
}
