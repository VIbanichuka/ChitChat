using ChitChat.Core.Entities;
using System;
using Microsoft.AspNetCore.SignalR;
using ChitChat.App.Server.Hubs.Interfaces;

namespace ChitChat.App.Server.Hubs
{
    public class ChitChatHub : Hub<IChitChatHubClient>
    {
    }
}
