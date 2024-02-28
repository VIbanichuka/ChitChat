namespace ChitChat.AngularApp.Server.Hubs.Interfaces
{
    public interface IChitChatHubClient
    {
        Task ReceiveMessageAsync(string message);
    }
}
