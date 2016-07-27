using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Attributes;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers.Abstract;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers
{
    [AdminOnly]
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

    }
}
