using System.ComponentModel.DataAnnotations;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Models.IDentity.RequestModels
{
    public class ApplicationUserRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } // Validated using UserNameValidator

        [Required]
        [StringLength(20, ErrorMessage = "User name must be between 6 and 50 characters in length", MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name must be between 1 and 50 characters in length", MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name must be between 1 and 50 characters in length", MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Level must be between 1 and 10")]
        public byte Level { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Role name must be between 5 and 256 characters in length", MinimumLength = 1)]
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
