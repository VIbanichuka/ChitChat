using System;
using Microsoft.AspNetCore.SignalR;
using ChitChat.App.Server.Hubs.Interfaces;
using ChitChat.App.Server.Models.Requests;
using ChitChat.App.Server.Models;
using ChitChat.App.Server.Services;

namespace ChitChat.App.Server.Hubs
{
    public class ChitChatHub : Hub<IChitChatHubClient>
    {
        private readonly IRedisService _redisService;

        public ChitChatHub(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task ReceiveMessageAsync(string message)
        {
            await Clients.All.ReceiveMessageAsync(message);
        }

        public async Task JoinGroupAsync(HubModel hubModel)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, hubModel.ChannelName!);
            await Clients.Group(hubModel.ChannelName!).ReceiveMessageAsync($"{hubModel.Sender} has joined the group");
        }

        public async Task SendMessageToGroupAsync(string channelName, string message, string sender)
        {
            if (string.IsNullOrEmpty(channelName) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(sender))
            {
                throw new ArgumentException("All arguments must be non-empty.");
            }

            var hubModel = new HubModel()
            {
                ChannelName = channelName,
                Message = message,
                Sender = sender,
                Timestamp = DateTimeOffset.UtcNow
            };
            await _redisService.StoreChannelMessageAsync(hubModel);
            await Clients.Group(hubModel.ChannelName).SendMessageAsync(hubModel.Message, hubModel.Sender, hubModel.ChannelName, hubModel.Timestamp);
        }

        public async Task RejoinGroupAsync(HubModel hubModel)
        {
            if (string.IsNullOrEmpty(hubModel.ChannelName))
            {
                throw new ArgumentException("Channel name cannot be empty.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, hubModel.ChannelName!);
            var chats = await _redisService.GetStoredChannelMessagesAsync(hubModel.ChannelName!);

            await SendValidMessagesAsync(chats);
        }

        private async Task SendValidMessagesAsync(IEnumerable<HubModel?> chats)
        {
            foreach (var chat in chats)
            {
                if (!string.IsNullOrEmpty(chat?.Message) && !string.IsNullOrEmpty(chat?.Sender) && !string.IsNullOrEmpty(chat?.ChannelName))
                {
                    await Clients.Caller.SendMessageAsync(chat.Message, chat.Sender, chat.ChannelName, chat.Timestamp);
                }
            }
        }

        public async Task ReconnectChannelAsync(HubModel hubModel)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, hubModel.ChannelName!);
        }

        public async Task LeaveGroupAsync(ChannelRequestModel channelRequest, UserRequestModel userRequest)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelRequest.ChannelName!);
            await Clients.Group(channelRequest.ChannelName!).ReceiveMessageAsync($"{userRequest.DisplayName} has left the group");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
