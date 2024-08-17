namespace ChitChat.App.Server.Models.Requests
{
    public class UserPublicKeyRequest
    {
        public Guid UserId { get; set; }
        public string PublicKey { get; set; }
    }
}
