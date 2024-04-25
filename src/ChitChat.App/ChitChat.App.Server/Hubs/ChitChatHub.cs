using ChitChat.Core.Entities;
using System;
using Microsoft.AspNetCore.SignalR;
using ChitChat.App.Server.Hubs.Interfaces;
using ChitChat.App.Server.Models.Requests;
using ChitChat.App.Server.Models;

namespace ChitChat.App.Server.Hubs
{
    public class ChitChatHub : Hub<IChitChatHubClient>
    {
        public async Task ReceiveMessageAsync(string message)
        {
            await Clients.All.ReceiveMessageAsync(message);
        }

        public async Task JoinGroupAsync(HubModel hubModel)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, hubModel.ChannelName!);
            await Clients.Group(hubModel.ChannelName!).ReceiveMessageAsync($"{hubModel.DisplayName} has joined the group");
        }

        public async Task SendMessageToGroupAsync(string channelName, string message, string sender)
        {
            await Clients.Group(channelName).SendMessageAsync(message, sender, channelName);
        }

        public async Task RejoinGroupAsync(HubModel hubModel)
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
