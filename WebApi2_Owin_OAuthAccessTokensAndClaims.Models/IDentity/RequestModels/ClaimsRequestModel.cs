using System.ComponentModel.DataAnnotations;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels
{
    public class ClaimsRequestModel
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
