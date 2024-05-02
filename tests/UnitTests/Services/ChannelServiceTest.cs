using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.App.Server.AutoMapperConfig;
using ChitChat.Application.Dtos;
using ChitChat.Application.Implementations;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using Moq;

namespace UnitTests.Services
{
    public class ChannelServiceTest
    {
        private readonly Mock<IChannelRepository> _mockChannelRepository;
        private readonly IMapper _mapper;
        private readonly IChannelService _channelService;
        private readonly Mock<IUserRepository> _mockUserRepository;

        public ChannelServiceTest()
        {
            var profile = new ChitChatProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(config);
            _mockChannelRepository = new Mock<IChannelRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _channelService = new ChannelService(_mockChannelRepository.Object, _mapper, _mockUserRepository.Object);
        }

        [Fact]
        public async Task CreateChannelAsync_ShouldAddChannelToDatabase()
        {
            var channelId = 1;
            var channel = new ChannelDto()
            {
                ChannelId = channelId,
                ChannelName = "TestChannel",
            };

            _mockChannelRepository.Setup(repo => repo.AddAsync(It.IsAny<Channel>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var result = await _channelService.CreateChannelAsync(channel);

            Assert.NotNull(result);
            Assert.IsType<ChannelDto>(result);
            Assert.Equal(channelId, result.ChannelId);
            Assert.Equal(channel.ChannelName, result.ChannelName);
            _mockChannelRepository.Verify(repo => repo.AddAsync(It.IsAny<Channel>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockChannelRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateChannelAsync_EmptyChannelName_ShouldThrowArgumentNullException()
        {
            var channelDto = new ChannelDto { ChannelName = "" };
            await Assert.ThrowsAsync<ArgumentNullException>(() => _channelService.CreateChannelAsync(channelDto));
        }

        [Fact]
        public async Task CreateChannelAsync_ShouldThrowArgumentExceptionIfChannelAlreadyExists()
        {
            var channelId = 1;
            var cancellationToken = CancellationToken.None;
            var existingChannel = new ChannelDto()
            {
                ChannelId = channelId,
                ChannelName = "TestChannel",
            };

            _mockChannelRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Channel, bool>>>())).ReturnsAsync(true);
            await Assert.ThrowsAsync<ArgumentException>(() => _channelService.CreateChannelAsync(existingChannel));
        }

        [Fact]
        public async Task JoinChannelByDisplayName_ChannelExistsAndUserExists_ShouldReturnTrueAndAddUserToChannel()
        {
            var channelId = 1;
            var displayName = "TestUser";
            var channel = new Channel { ChannelId = channelId };
            var user = new User { UserId = Guid.NewGuid(), DisplayName = displayName };

            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync(channel);
            
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _channelService.JoinChannelByDisplayName(channelId, displayName);

            Assert.True(result);
            Assert.Contains(user, channel.Users);
            _mockChannelRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task JoinChannelByDisplayName_ChannelDoesNotExist_ShouldThrowArgumentNullException()
        {
            var channelId = 1;
            var displayName = "TestUser";
            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync((Channel)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _channelService.JoinChannelByDisplayName(channelId, displayName));
        }

        [Fact]
        public async Task JoinChannel_ValidUserIdAndChannelId_ShouldAddUserToChannel()
        {
            var channelId = 1;
            var userId = Guid.NewGuid();
            var channel = new Channel 
            { 
                ChannelId = channelId
            };
            var user = new User 
            { 
                UserId = userId, 
                DisplayName = "TestUser"
            };

            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync(channel);

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _channelService.JoinChannel(channelId, userId);

            Assert.True(result);
            Assert.Contains(user, channel.Users);
            _mockChannelRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task LeaveChannel_ValidUserIdAndChannelId_ShouldRemoveUserFromChannel()
        {
            var channelId = 1;
            var userId = Guid.NewGuid();
            var channel = new Channel
            {
                ChannelId = channelId
            };
            var user = new User
            {
                UserId = userId,
                DisplayName = "TestUser"
            };

            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync(channel);

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var result = await _channelService.LeaveChannel(channelId, userId);

            Assert.True(result);
            Assert.DoesNotContain(user, channel.Users);
            _mockChannelRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task LeaveChannel_ChannelDoesNotExist_ShouldThrowArgumentNullException()
        {
            var channelId = 1;
            var userId = Guid.NewGuid();
            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync((Channel)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _channelService.LeaveChannel(channelId, userId));
        }

        [Fact]
        public async Task LeaveChannel_UserDoesNotExist_ShouldThrowArgumentNullException()
        {
            var channelId = 1;
            var userId = Guid.NewGuid();
            var channel = new Channel { ChannelId = channelId };

            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync(channel);
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _channelService.LeaveChannel(channelId, userId));
        }


        [Fact]
        public async Task GetChannelsByUserIdAsync_UserExists_ShouldReturnListOfChannels()
        {
            var userId = Guid.NewGuid();
            var userChannels = new List<Channel>()
            {
                new Channel()
                { 
                    ChannelId = 1, 
                    ChannelName = "Channel 1" 
                },
                new Channel()
                { 
                    ChannelId = 2, ChannelName = "Channel 2" 
                }
            };

            _mockChannelRepository.Setup(repo => repo.GetChannelsByUserIdAsync(userId)).ReturnsAsync(userChannels);

            var result = await _channelService.GetChannelsByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userChannels.Count, result.Count());
            foreach (var channel in userChannels)
            {
                Assert.Contains(result, c => c.ChannelId == channel.ChannelId && c.ChannelName == channel.ChannelName);
            }
        }

        [Fact]
        public async Task GetChannelsByUserIdAsync_UserDoesNotExist_ShouldReturnEmptyList()
        {
            var userId = Guid.NewGuid();
            _mockChannelRepository.Setup(repo => repo.GetChannelsByUserIdAsync(userId)).ReturnsAsync(new List<Channel>());

            var result = await _channelService.GetChannelsByUserIdAsync(userId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetChannelMembersAsync_ChannelExists_ShouldReturnListOfMembers()
        {
            var channelId = 1;
            var channel = new Channel
            {
                ChannelId = channelId,
                Users = new List<User>()
                {
                    new User() 
                    { 
                        UserId = Guid.NewGuid(), 
                        DisplayName = "User 1" 
                    },
                    new User() 
                    { 
                        UserId = Guid.NewGuid(), 
                        DisplayName = "User 2" 
                    }
                }
            };
            _mockChannelRepository.Setup(repo => repo.GetChannelWithUserByIdAsync(channelId)).ReturnsAsync(channel);

            var result = await _channelService.GetChannelMembersAsync(channelId);

            Assert.NotNull(result);
            Assert.Equal(channel.Users.Count, result.Count());
            foreach (var user in channel.Users)
            {
                Assert.Contains(result, m => m.UserId == user.UserId && m.DisplayName == user.DisplayName);
            }
        }

        [Fact]
        public async Task UpdateChannelAsync_ChannelExists_ShouldReturnUpdatedChannel()
        {
            var channelId = 1;
            var channelDto = new ChannelDto() 
            {   ChannelId = channelId, 
                ChannelName = "Updated Channel Name" 
            };
            var existingChannel = new Channel() 
            { 
                ChannelId = channelId, 
                ChannelName = "Original Channel Name" 
            };

            _mockChannelRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Channel, bool>>>(), default)).ReturnsAsync(existingChannel);

            var result = await _channelService.UpdateChannelAsync(channelDto);

            Assert.NotNull(result);
            Assert.Equal(channelDto.ChannelId, result.ChannelId);
            Assert.Equal(channelDto.ChannelName, result.ChannelName);
            _mockChannelRepository.Verify(repo => repo.Update(existingChannel), Times.Once);
            _mockChannelRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateChannelAsync_ChannelDoesNotExist_ShouldThrowArgumentNullException()
        {
            var channelId = 1;
            var channelDto = new ChannelDto { ChannelId = channelId, ChannelName = "Updated Channel Name" };

            _mockChannelRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Channel, bool>>>(), default)).ReturnsAsync((Channel)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _channelService.UpdateChannelAsync(channelDto));
        }

    }
}
