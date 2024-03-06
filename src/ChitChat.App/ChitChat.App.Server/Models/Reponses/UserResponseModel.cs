namespace ChitChat.App.Server.Models.Reponses
{
    public class UserResponseModel
    {
        public Guid UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }
}
