using System.Reflection;
using ChitChat.App.Server.Models;
using ChitChat.Core.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChitChat.App.Server.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public readonly IDatabase _database;
        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            
        }

        public async Task<List<HubModel?>> GetStoredChannelMessagesAsync(string channelName)
        {
            var listKey = $"{channelName}:messages";
            var messages = await _database.ListRangeAsync(listKey);
            return messages.Select(m => m.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<HubModel>(m!)).ToList();
        }

        public async Task StoreChannelMessageAsync(HubModel hubModel)
        {
            var listKey = $"{hubModel.ChannelName}:messages";
            var messageJson = JsonConvert.SerializeObject(hubModel);
            await _database.ListRightPushAsync(listKey, messageJson);
            await _database.KeyExpireAsync(listKey, TimeSpan.FromDays(1));
        }
    }
}
