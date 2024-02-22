using System.ComponentModel.DataAnnotations;

namespace ChitChat.Web.Models.Requests
{
    public class UserModel
    {
        [Required(ErrorMessage = "Field required")]
        [MaxLength(30)]
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
