namespace ChitChat.App.Server.Models.Requests
{
    public class UserKeysRequest
    {
        public Guid UserId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
