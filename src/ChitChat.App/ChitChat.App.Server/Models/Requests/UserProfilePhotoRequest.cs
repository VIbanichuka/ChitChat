using System.ComponentModel.DataAnnotations;

namespace ChitChat.App.Server.Models.Requests
{
    public class UserProfilePhotoRequest
    {
        [Required(ErrorMessage = "Upload an image")]
        public IFormFile ImageFile { get; set; }
    }
}
