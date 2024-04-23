using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.App.Server.AutoMapperConfig;
using ChitChat.Application.Implementations;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using ChitChat.Core.Enums;
using Moq;

namespace UnitTests.Services
{
    public class FriendshipServiceTest
    {
        private readonly Mock<IFriendshipRepository> _mockFriendshipRepository;
        private readonly IMapper _mapper;
        private readonly IFriendshipService _friendshipService;

        public FriendshipServiceTest()
        {
            var profile = new ChitChatProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(config);
            _mockFriendshipRepository = new Mock<IFriendshipRepository>();
            _friendshipService = new FriendshipService(_mockFriendshipRepository.Object, _mapper);
        }

        [Fact]
        public async Task AcceptFriendRequestAsync_WithPendingFriendship_ShouldChangeStatusToAccepted()
        {
            var friendshipId = 1;
            var friendship = new Friendship()
            {
                FriendshipId = friendshipId,
                FriendshipStatus = FriendshipStatus.Pending,
            };

            _mockFriendshipRepository.Setup(repo => repo
                .FindAsync(It.IsAny<Expression<Func<Friendship, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(friendship);

            await _friendshipService.AcceptFriendRequestAsync(friendshipId);

            Assert.Equal(FriendshipStatus.Accepted, friendship.FriendshipStatus);
            _mockFriendshipRepository.Verify(repo => repo.Update(friendship), Times.Once);
            _mockFriendshipRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RejectFriendRequestAsync_WithPendingFriendship_ShouldChangeStatusToRejected()
        {
            var friendshipId = 1;
            var friendship = new Friendship()
            {
                FriendshipId = friendshipId,
                FriendshipStatus = FriendshipStatus.Pending,
            };

            _mockFriendshipRepository.Setup(repo => repo
                .FindAsync(It.IsAny<Expression<Func<Friendship, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(friendship);

            await _friendshipService.RejectFriendRequestAsync(friendshipId);

            Assert.Equal(FriendshipStatus.Rejected, friendship.FriendshipStatus);
            _mockFriendshipRepository.Verify(repo => repo.Update(friendship), Times.Once);
            _mockFriendshipRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SendFriendRequestAsync_WithValidInput_ShouldCreateFriendship()
        {
            var inviterId = Guid.NewGuid();
            var inviteeId = Guid.NewGuid();

            _mockFriendshipRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Friendship, bool>>>())).ReturnsAsync(false);

            await _friendshipService.SendFriendRequestAsync(inviterId, inviteeId);

            _mockFriendshipRepository.Verify(repo => repo.AddAsync(It.IsAny<Friendship>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockFriendshipRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SendFriendRequestAsync_InviterAndInviteeAreSame_ShouldThrowArgumentException()
        {
            var inviterId = Guid.NewGuid();
            var inviteeId = inviterId;

            _mockFriendshipRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Friendship, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _friendshipService.SendFriendRequestAsync(inviterId, inviteeId));
        }

        [Fact]
        public async Task SendFriendRequestAsync_FriendshipExists_ShouldThrowInvalidOperationException()
        {
            var inviterId = Guid.NewGuid();
            var inviteeId = Guid.NewGuid();

            _mockFriendshipRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Friendship, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _friendshipService.SendFriendRequestAsync(inviterId, inviteeId));
        }
    }
}
