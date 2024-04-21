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
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Services
{
    public class UserProfileServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUserProfileRepository> _mockUserProfileRepository;
        private readonly IUserProfileService _userProfileService;
        private readonly Mock<IFileService> _mockFileService;

        public UserProfileServiceTest()
        {
            var profile = new ChitChatProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            _mapper = new Mapper(config);
            _mockUserProfileRepository = new Mock<IUserProfileRepository>();
            _mockFileService = new Mock<IFileService>();
            _userProfileService = new UserProfileService(_mockUserProfileRepository.Object, _mapper, _mockFileService.Object);
        }

        [Fact]
        public async Task GetAllUserProfilesAsync_ShouldReturnAllUserProfiles()
        {
            var userProfileId = Guid.NewGuid();
            var userProfiles = new List<UserProfile>()
            {
                new UserProfile
                {
                    FirstName= "John",
                    LastName="Doe",
                    Bio= "Artist",
                },
                new UserProfile
                {
                    FirstName= "Jane",
                    LastName="Doe",
                    Bio= "Stylist",
                },
            };

            _mockUserProfileRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(userProfiles);
            var result = await _userProfileService.GetAllUserProfilesAsync();
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<UserProfileDto>>(result);
        }

        [Fact]
        public async Task GetAllUserProfilesAsync__ShouldReturnEmptyCollection_WhenNoData()
        {
            var userProfiles = new List<UserProfile>();
            _mockUserProfileRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(userProfiles);

            var result = await _userProfileService.GetAllUserProfilesAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUserProfileByIdAsync_EmptyId_ThrowsArgumentNullException()
        {
            var userId = Guid.Empty;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userProfileService.GetUserProfileByIdAsync(userId));
        }

        [Fact]
        public async Task GetUserProfileByIdAsync_ShouldReturnUserProfile()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5");
            var userProfile = new UserProfile()
            {
                FirstName = "John",
                LastName = "Doe",
                Bio = "Artist",
            };
            _mockUserProfileRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(userProfile);
            var expectedUserProfile = new UserProfile()
            {
                FirstName = "John",
                LastName = "Doe",
                Bio = "Artist",
            };
            
            var result = await _userProfileService.GetUserProfileByIdAsync(userId);

            Assert.NotNull(result);
            Assert.IsType<UserProfileDto>(result);
            Assert.Equal(expectedUserProfile.FirstName, result.FirstName);
            Assert.Equal(expectedUserProfile.LastName, result.LastName);
            Assert.Equal(expectedUserProfile.Bio, result.Bio);
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldUpdateAndReturnUpdatedUserProfile()
        {
            var userProfileId = Guid.NewGuid();
            var existingUserProfile = new UserProfile()
            {
                UserProfileId = userProfileId,
                FirstName = "John",
                LastName = "Doe",
                Bio = "Artist",
            };

            var updatedUserProfile = new UserProfileDto()
            {
                UserProfileId = userProfileId,
                FirstName = "John",
                LastName = "Doe",
                Bio = "Student"
            };

            _mockUserProfileRepository.Setup(repo => repo.GetByIdAsync(userProfileId)).ReturnsAsync(existingUserProfile);

            var result = await _userProfileService.UpdateUserProfileAsync(updatedUserProfile);

            Assert.NotNull(result);
            Assert.Equal(updatedUserProfile.UserProfileId, result.UserProfileId);
            Assert.Equal(updatedUserProfile.FirstName, result.FirstName);
            Assert.Equal(updatedUserProfile.LastName, result.LastName);
            Assert.Equal(updatedUserProfile.Bio, result.Bio);

            _mockUserProfileRepository.Verify(repo => repo.Update(It.IsAny<UserProfile>()), Times.Once);
            _mockUserProfileRepository.Verify(repo => repo.Update(It.Is<UserProfile>(p => p.UserProfileId == userProfileId)), Times.Once);
            _mockUserProfileRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UploadProfilePhoto_WithValidUserProfile_ShouldUpdateProfilePicture()
        {
            var userProfileId = Guid.NewGuid();

            var userProfile = new UserProfileDto()
            {
                UserProfileId = userProfileId,
                ImageFile = Mock.Of<IFormFile>()
            };

            var existingUserProfile = new UserProfile
            {
                UserProfileId = userProfileId,
                FirstName = "John",
                LastName = "Doe",
                Bio = "Student"
            };

            var uploadedFileName = "uploaded_file_3fa85f64-5717-4562-b3fc-2c963f66afa5.jpg";

            _mockUserProfileRepository.Setup(repo => repo.GetByIdAsync(userProfileId)).ReturnsAsync(existingUserProfile);
            _mockFileService.Setup(fileService => fileService.UploadImageAsync(It.IsAny<IFormFile>())).ReturnsAsync(uploadedFileName);

            var result = await _userProfileService.UploadProfilePhoto(userProfile);
            
            Assert.NotNull(result);
            Assert.Equal(userProfileId, result.UserProfileId);
            Assert.Equal(uploadedFileName, existingUserProfile.ProfilePicture);
            _mockUserProfileRepository.Verify(repo => repo.Update(existingUserProfile), Times.Once);
            _mockUserProfileRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProfilePhotoAsync_WithValidUserProfile_ShouldDeleteProfilePhoto()
        {
            var userProfileId = Guid.NewGuid();
            var userProfile = new UserProfileDto()
            {
                UserProfileId = userProfileId,
            };

            var existingUserProfile = new UserProfile
            {
                UserProfileId = userProfileId,
                FirstName = "John",
                LastName = "Doe",
                Bio = "Student",
                ProfilePicture = "uploaded_file_3fa85f64-5717-4562-b3fc-2c963f66afa5.jpg"
            };

            _mockUserProfileRepository.Setup(repo => repo.GetByIdAsync(userProfileId)).ReturnsAsync(existingUserProfile);

            var result = await _userProfileService.DeleteProfilePhotoAsync(userProfile);

            Assert.True(result);
            Assert.Empty(existingUserProfile.ProfilePicture);
            _mockFileService.Verify(fileService => fileService.RemoveImage("uploaded_file_3fa85f64-5717-4562-b3fc-2c963f66afa5.jpg"), Times.Once);
            _mockUserProfileRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProfilePhotoAsync_WithNoUserProfile_ShouldThrowArgumentNullException()
        {
            var userProfile = new UserProfileDto() 
            { 
                UserProfileId = Guid.NewGuid()
            };

            _mockUserProfileRepository.Setup(repo => repo.GetByIdAsync(userProfile.UserProfileId)).ReturnsAsync((UserProfile)null!);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _userProfileService.DeleteProfilePhotoAsync(userProfile));
        }
    }
}
