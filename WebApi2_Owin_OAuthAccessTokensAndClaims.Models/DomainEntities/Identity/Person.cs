using System;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.DomainEntities.Identity
{
    public class Person
    {
        #region NAVIGATION PROPERTIES
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        #endregion

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Level { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
