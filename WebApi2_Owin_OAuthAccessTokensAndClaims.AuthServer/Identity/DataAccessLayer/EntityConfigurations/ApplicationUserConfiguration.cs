using System.Data.Entity.ModelConfiguration;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Identity.Entities;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.Identity.DataAccessLayer.EntityConfigurations
{
    /// <summary>
    /// Configuration settings for the 'ApplicationUser' entity set.
    /// </summary>
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            // Relationship with 'Person' entity defined in 'PersonConfiguration'
        }
    }
}
