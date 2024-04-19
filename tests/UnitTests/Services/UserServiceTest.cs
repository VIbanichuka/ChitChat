using System.Linq.Expressions;
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
    public class UserServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IUserService _userService;

        public UserServiceTest()
        {
            var profile = new ChitChatProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(config);
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnMappedUsers()
        {
            var userId = Guid.NewGuid();
            var users = new List<User>()
            {
                new User
                {
                    UserId = userId,
                    DisplayName = "TestUser1",
                    Email = "testUser1@gmail.com"
                },
                new User
                {
                    UserId = userId,
                    DisplayName = "TestUser2",
                    Email = "testUser2@gmail.com"
                }
            };
            _mockUserRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
            var result = await _userService.GetAllUsersAsync();
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyCollection_WhenRepositoryReturnsEmpty()
        {
            var users = new List<User>();
            _mockUserRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5");
            var user = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var expectedUser = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal(expectedUser.DisplayName, result.DisplayName);
            Assert.Equal(expectedUser.UserId, result.UserId);
        }
        
        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser()
        {
            var userId = Guid.NewGuid();
            var email = "testUser1@gmail.com";
            var user = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = email,
            };
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var expectedUser = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com",
            };

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(expectedUser.DisplayName, result.DisplayName);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetUserByEmailAsync_NullOrEmptyEmail_ThrowsArgumentNullException(string email)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.GetUserByEmailAsync(email));
        }

        [Fact]
        public async Task GetUserByEmailAsync_EmailNotFound_ReturnsNull()
        {
            var email = "nonexistent@gmail.com";
            _mockUserRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((User)null!);

            var result = await _userService.GetUserByEmailAsync(email);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToDatabase()
        {
            var userId = Guid.NewGuid();
            var user = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.AddAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _userService.CreateUserAsync(user);

            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnThrowArgumentExceptionIfUserAlreadyExists()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var existingUser = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(existingUser));
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ShouldUpdateAndReturnUpdatedUser()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };

            var updatedUser = new UserDto()
            {
                UserId = userId,
                DisplayName = "Blue",
                Email = "blue@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);

            var result = await _userService.UpdateUserAsync(updatedUser);

            Assert.NotNull(result);
            Assert.Equal(updatedUser.UserId, result.UserId);
            Assert.Equal(updatedUser.Email, result.Email);
            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUser_ShouldThrowException()
        {
            var userId = Guid.NewGuid();
            var nonExistingUser = new UserDto
            {
                UserId = userId,
                DisplayName = "NewTestUser",
                Email = "newTestUser@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.UpdateUserAsync(nonExistingUser));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteExistingUserFromDatabase()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User()
            {
                UserId = userId,
                DisplayName = "TestUser1",
                Email = "testUser1@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            var result = await _userService.DeleteUserAsync(userId);

            Assert.True(result);

            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalseIfNoUserExist()
        {
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null!);
            var result = await _userService.DeleteUserAsync(userId);

            Assert.False(result);

            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<User>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
