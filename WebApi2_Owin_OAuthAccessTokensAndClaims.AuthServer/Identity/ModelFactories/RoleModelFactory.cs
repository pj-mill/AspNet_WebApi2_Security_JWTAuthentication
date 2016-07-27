using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Web.Http.Routing;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.ResponseModels;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.ModelFactories
{
    public class RoleModelFactory
    {
        private UrlHelper _UrlHelper;

        public RoleModelFactory(HttpRequestMessage request)
        {
            _UrlHelper = new UrlHelper(request);
        }

        public RoleResponseModel Create(IdentityRole appRole)
        {
            return new RoleResponseModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }
    }
}
