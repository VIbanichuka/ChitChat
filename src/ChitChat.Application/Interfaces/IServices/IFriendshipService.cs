using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IFriendshipService
    {
        Task SendFriendRequest(Guid inviterId, Guid inviteeId);
        Task AcceptFriendRequest(int friendshipId);
        Task RejectFriendRequest(int friendshipId);
    }
}
