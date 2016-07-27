using System.ComponentModel.DataAnnotations;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels
{
    public class ApplicationUserRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public byte Level { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The Password must be at least 6 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
