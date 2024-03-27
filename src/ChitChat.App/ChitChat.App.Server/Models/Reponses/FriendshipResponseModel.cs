using ChitChat.Application.Dtos;

namespace ChitChat.App.Server.Models.Reponses
{
    public class FriendshipResponseModel
    {
        public string? DisplayName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime? InviteTime { get; set; } = DateTime.UtcNow;
    }
}
