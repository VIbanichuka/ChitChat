namespace ChitChat.Web.Hubs.Interfaces
{
    public interface IChitChatHubClient
    {
        Task ReceiveMessageAsync(string message);
    }
}
