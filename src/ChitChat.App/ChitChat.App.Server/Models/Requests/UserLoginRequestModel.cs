using System.ComponentModel.DataAnnotations;

namespace ChitChat.App.Server.Models.Requests
{
    public class UserLoginRequestModel
    {
        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
