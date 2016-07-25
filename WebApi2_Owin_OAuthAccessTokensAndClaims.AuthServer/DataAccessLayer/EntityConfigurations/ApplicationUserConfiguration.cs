using System.Data.Entity.ModelConfiguration;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.DataAccessLayer.EntityConfigurations
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
