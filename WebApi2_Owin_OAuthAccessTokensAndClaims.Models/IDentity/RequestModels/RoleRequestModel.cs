using System.ComponentModel.DataAnnotations;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels
{
    class RoleRequestModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The Role name must be at least {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
