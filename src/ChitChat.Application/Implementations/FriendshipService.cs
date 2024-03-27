using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using ChitChat.Core.Enums;

namespace ChitChat.Application.Implementations
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IMapper _mapper;

        public FriendshipService(IFriendshipRepository friendshipRepository, IMapper mapper)
        {
            _friendshipRepository = friendshipRepository;
            _mapper = mapper;
        }

        public async Task AcceptFriendRequestAsync(int friendshipId)
        {
            var friendship = await _friendshipRepository.FindAsync(f => f.FriendshipId == friendshipId);
            if (friendship != null && friendship.FriendshipStatus == FriendshipStatus.Pending)
            {
                friendship.FriendshipStatus = FriendshipStatus.Accepted;
                _friendshipRepository.Update(friendship);
                await _friendshipRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FriendDto>> GetAllFriendsAsync(Guid userId)
        {
            var friendships = await _friendshipRepository
                .GetAllWithIncludeAsync(f => (f.InviteeId == userId || f.InviterId == userId) && f.FriendshipStatus == FriendshipStatus.Accepted, 
                    f => f.Inviter, f => f.Invitee, f => f.Inviter.UserProfile, f => f.Invitee.UserProfile);

            var friends = friendships.SelectMany(friendship =>
            {
                var friend = friendship.InviteeId == userId ? friendship.Inviter : friendship.Invitee;
                return new[]
                {
                    new FriendDto()
                    {
                        UserId = friend.UserId,
                        DisplayName = friend.DisplayName,
                        ProfilePicture = friend.UserProfile.ProfilePicture
                    }
                };
            }).ToList();

            return friends;
        }

        public async Task<IEnumerable<FriendshipDto>> GetPendingFriendRequestsAsync(Guid userId)
        {
            var pendingFriendships = await _friendshipRepository
                .GetAllWithIncludeAsync(f => (f.InviteeId == userId) && f.FriendshipStatus == FriendshipStatus.Pending, 
                    f => f.Inviter, f => f.Invitee, f => f.Inviter.UserProfile, f => f.Invitee.UserProfile);
            var pendingFriendshipDtos = pendingFriendships.SelectMany(pendingFriendship => 
            {
                var pendingInvite = pendingFriendship.InviteeId == userId ? pendingFriendship.Inviter : pendingFriendship.Invitee;
                return new[]
                {
                    new FriendshipDto()
                    {
                        FriendshipId = pendingFriendship.FriendshipId,
                        InviteeId = pendingFriendship.InviteeId,
                        InviteTime = pendingFriendship.InviteTime,
                        InviterId = pendingFriendship.InviterId,
                        FriendshipStatus = pendingFriendship.FriendshipStatus,
                        DisplayName = pendingInvite.DisplayName,
                        ProfilePicture = pendingInvite.UserProfile.ProfilePicture
                    }
                };
            }).ToList();
            return pendingFriendshipDtos;
        }

        public async Task RejectFriendRequestAsync(int friendshipId)
        {
            var friendship = await _friendshipRepository.FindAsync(f => f.FriendshipId == friendshipId);
            if (friendship != null && friendship.FriendshipStatus == FriendshipStatus.Pending)
            {
                friendship.FriendshipStatus = FriendshipStatus.Rejected;
                _friendshipRepository.Update(friendship);
                await _friendshipRepository.SaveChangesAsync();
            }
        }

        public async Task SendFriendRequestAsync(Guid inviterId, Guid inviteeId)
        {
            if (inviterId == inviteeId)
            {
                throw new ArgumentException("Inviter and invitee cannot be the same.");
            }

            if (await CheckIfFriendshipExistsAsync(inviterId, inviteeId))
            {
                throw new InvalidOperationException("Friendship request already exists.");
            }

            var friendship = new Friendship()
            {
                InviteeId = inviteeId,
                InviterId = inviterId,
                FriendshipStatus = FriendshipStatus.Pending,
            };
            await _friendshipRepository.AddAsync(friendship);
            await _friendshipRepository.SaveChangesAsync();
        }

        private async Task<bool> CheckIfFriendshipExistsAsync(Guid inviterId, Guid inviteeId)
        {
            return await _friendshipRepository.AnyAsync(f => (f.InviterId == inviterId && f.InviteeId == inviteeId) || (f.InviterId == inviteeId && f.InviteeId == inviterId));
        }
    }
}
