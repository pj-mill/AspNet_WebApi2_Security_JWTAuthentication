using System;
using System.Net.Http;
using System.Web.Http.Routing;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Managers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.ResponseModels;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.ModelFactories
{
    public class ApplicationUserModelFactory
    {
        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ApplicationUserModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        /// <summary>
        /// Converts a user entity model to a response model
        /// </summary>
        /// <param name="apuserpUser"></param>
        /// <returns></returns>
        public ApplicationUserResponseModel Create(ApplicationUser user)
        {
            return new ApplicationUserResponseModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = user.Id }),
                Id = user.Id,
                UserName = user.UserName,
                FullName = $"{user.Person.FirstName} {user.Person.LastName}",
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Level = user.Person.Level,
                JoinDate = user.Person.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(user.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(user.Id).Result
            };
        }

        /// <summary>
        /// Converts a user request model into an entity model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ApplicationUser Create(ApplicationUserRequestModel user)
        {
            Person p = new Person
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JoinDate = DateTime.Now,
                Level = user.Level
            };

            return new ApplicationUser { UserName = user.Username, Email = user.Email, Person = p };
        }
    }
}
