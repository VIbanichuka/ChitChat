namespace ChitChat.App.Server.Hubs.Interfaces
{
    public interface IChitChatHubClient
    {
        Task ReceiveMessageAsync(string message);
    }
}
