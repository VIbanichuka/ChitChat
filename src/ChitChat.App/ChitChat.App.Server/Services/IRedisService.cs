using ChitChat.App.Server.Models;

namespace ChitChat.App.Server.Services
{
    public interface IRedisService
    {
        Task StoreChannelMessageAsync(HubModel hubModel);
        Task<List<HubModel?>> GetStoredChannelMessagesAsync(string channelName);
    }
}
