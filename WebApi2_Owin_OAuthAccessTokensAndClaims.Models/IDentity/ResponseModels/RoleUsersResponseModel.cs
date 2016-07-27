using System.Collections.Generic;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.ResponseModels
{
    public class RoleUsersResponseModel
    {
        public string Id { get; set; }
        public List<string> EnrolledUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }
}
