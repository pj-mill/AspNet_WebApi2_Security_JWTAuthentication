using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Attributes;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Helpers;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        /// <summary>
        /// Testing the 'ClaimsAuthorizationAttribute' class
        /// </summary>
        /// <returns></returns>
        [ClaimsAuthorization(ClaimType = "FTE", ClaimValue = "1")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }

        /// <summary>
        /// Testing the 'IncidentResolver' claim
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Authorize(Roles = RoleNames.IncidentResolver)]
        [HttpPut]
        [Route("refund/{orderId}")]
        public IHttpActionResult RefundOrder([FromUri]string orderId)
        {
            return Ok();
        }
    }
}
