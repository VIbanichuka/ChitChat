namespace ChitChat.App.Server.Models.Reponses
{
    public class FriendResponse
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
