using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Interfaces.IServices;

namespace ChitChat.Application.Implementations
{
    public class FriendshipService : IFriendshipService
    {
        public Task AcceptFriendRequest(int friendshipId)
        {
            throw new NotImplementedException();
        }

        public Task RejectFriendRequest(int friendshipId)
        {
            throw new NotImplementedException();
        }

        public Task SendFriendRequest(Guid inviterId, Guid inviteeId)
        {
            throw new NotImplementedException();
        }
    }
}
