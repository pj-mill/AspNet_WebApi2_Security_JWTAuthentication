using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.DataAccessLayer.EntityConfigurations
{
    /// <summary>
    /// Configuration settings for the 'Person' entity set.
    /// </summary>
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            // N.B. ApplicationUser relationionship defined in 'ApplicationUserConfiguration'

            // Define Primary Key
            HasKey(p => p.ApplicationUserId);

            // Property configurations
            Property(p => p.ApplicationUserId).HasMaxLength(128);
            Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            Property(p => p.LastName).IsRequired().HasMaxLength(50);
            Property(p => p.Level).IsRequired();
            Property(p => p.JoinDate).IsRequired();

            // Indexes
            Property(p => p.FirstName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IDX_PERSON_FIRSTNAME") { IsUnique = false }));
            Property(p => p.LastName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IDX_PERSON_LASTNAME") { IsUnique = false }));

            this.HasRequired(a => a.ApplicationUser).WithRequiredDependent(a => a.Person);
        }
    }
}
