using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.DataAccessLayer.EntityConfigurations;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.AuthServer.DataAccessLayer.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region DBSETS
        public virtual DbSet<Person> People { get; set; }
        #endregion

        #region CONSTRUCTORS
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
