using System.ComponentModel.DataAnnotations;

namespace ChitChat.App.Server.Models.Requests
{
    public class ChangePasswordRequestModel
    {

        [Required, MinLength(6)]
        public string CurrentPassword { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
