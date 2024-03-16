namespace ChitChat.App.Server.Models.Reponses
{
    public class UserProfileResponseModel
    {
        public Guid UserId { get; set; }
        public Guid UserProfileId { get; set; }
        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
