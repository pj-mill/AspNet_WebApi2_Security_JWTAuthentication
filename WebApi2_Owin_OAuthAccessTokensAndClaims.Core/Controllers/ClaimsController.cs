using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers.Abstract;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Core.Controllers
{
    [RoutePrefix("api/claims")]
    public class ClaimsController : BaseApiController
    {
        /// <summary>
        /// Responsible for unpacking claims in the JWT and returning them
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult GetClaims()
        {
            var identity = User.Identity as ClaimsIdentity;

            var claims = from c in identity.Claims
                         select new
                         {
                             subject = c.Subject.Name,
                             type = c.Type,
                             value = c.Value
                         };

            return Ok(claims);
        }
    }
}
