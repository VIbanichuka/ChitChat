using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(Guid inviterId, Guid inviteeId);
        Task AcceptFriendRequestAsync(int friendshipId);
        Task RejectFriendRequestAsync(int friendshipId);
        Task<IEnumerable<FriendDto>> GetAllFriendsAsync(Guid userId);
        Task<IEnumerable<Friendship>> GetPendingFriendRequestsAsync(Guid userId);
    }
}
