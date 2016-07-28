using System;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Client
{
    public class User : IEquatable<User>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Token Token { get; set; }

        public bool Equals(User other)
        {
            return this.UserName.Equals(other.UserName);
        }
    }
}
