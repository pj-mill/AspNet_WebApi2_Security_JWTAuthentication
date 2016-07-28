using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.Contexts;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Helpers;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.Claims
{
    /// <summary>
    /// Responsible for generating claims for app users
    /// </summary>
    public class ApplicationClaimsFactory : ClaimsIdentityFactory<ApplicationUser>
    {
        private ApplicationDbContext dbContext;

        public ApplicationClaimsFactory()
        {
            // Customize the type names of claims (just to simplify them)
            base.UserNameClaimType = "username";
            base.RoleClaimType = "role";
        }

        public ApplicationClaimsFactory(ApplicationDbContext dbContext) : base()
        {
            this.dbContext = dbContext;
        }

        public override async Task<ClaimsIdentity> CreateAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user, string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);

            if (user.Person == null)
            {
                user.Person = await dbContext.People.FindAsync(user.Id);
            }

            /*---------------------------------------------------------------------------------
            Authorization level claim
            ---------------------------------------------------------------------------------*/
            ci.AddClaim(CreateClaim("level", user.Person.Level.ToString()));

            /*---------------------------------------------------------------------------------
            Full Time Employee Claim
            ---------------------------------------------------------------------------------*/
            var daysInWork = (DateTime.Now.Date - user.Person.JoinDate).TotalDays;

            if (daysInWork > 90)
            {
                ci.AddClaim(CreateClaim("FTE", "1"));
            }
            else
            {
                ci.AddClaim(CreateClaim("FTE", "0"));
            }

            /*---------------------------------------------------------------------------------
            Incident Resolver claim
            ---------------------------------------------------------------------------------*/
            if (ci.HasClaim(x => x.Type == ClaimTypes.Role && x.Value.Equals("Admin")))
            {
                if (daysInWork > 90)
                {
                    ci.AddClaim(CreateClaim(ClaimTypes.Role, RoleNames.IncidentResolver));
                }
            }

            return ci;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
