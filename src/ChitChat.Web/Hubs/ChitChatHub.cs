using ChitChat.Core.Entities;
using System;
using Microsoft.AspNetCore.SignalR;
using ChitChat.Web.Hubs.Interfaces;

namespace ChitChat.Web.Hubs
{
    public class ChitChatHub : Hub<IChitChatHubClient>
    {
        public async Task BroadcastMessage(string message)
        {
            await Clients.All.ReceiveMessageAsync(message);
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
