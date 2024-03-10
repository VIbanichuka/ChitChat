using System.ComponentModel.DataAnnotations;

namespace ChitChat.App.Server.Models.Requests
{
    public class SendFriendRequestModel
    {
        [Required]
        public Guid InviterId { get; set; }

        [Required]
        public Guid InviteeId { get; set; }
    }
}
