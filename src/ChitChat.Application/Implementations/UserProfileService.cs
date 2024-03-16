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

namespace ChitChat.Application.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public UserProfileService(IUserProfileRepository userProfileRepository, IMapper mapper, IFileService fileService)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);   
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.FindAsync(u => u.UserId == userId);
            return _mapper.Map<UserProfileDto>(userProfile);
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(UserProfileDto userProfile)
        {
           var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfile.UserProfileId);

            if (existingUserProfile == null)
            {
                throw new ArgumentNullException(nameof(userProfile));
            }

            _mapper.Map(userProfile, existingUserProfile);
            _userProfileRepository.Update(existingUserProfile);
            await _userProfileRepository.SaveChangesAsync();
            return _mapper.Map<UserProfileDto>(existingUserProfile);
        }

        public async Task<UserProfileDto> EditProfilePhoto(UserProfileDto userProfile) 
        {
            var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfile.UserProfileId);
            if (existingUserProfile == null) 
            {
                throw new ArgumentNullException(nameof(userProfile));
            }
            await _fileService.UpdateImageAsync(userProfile.ImageFile, existingUserProfile.ProfilePicture);

            _mapper.Map(userProfile, existingUserProfile);      
            _userProfileRepository.Update(existingUserProfile);
            await _userProfileRepository.SaveChangesAsync();
            return _mapper.Map<UserProfileDto>(existingUserProfile);
        }

        public async Task<bool> DeleteProfilePhotoAsync(UserProfileDto userProfile)
        {
            var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfile.UserProfileId);
            if (existingUserProfile == null)
            {
                throw new ArgumentNullException(nameof(userProfile));
            }
            _fileService.RemoveImage(existingUserProfile.ProfilePicture);
            await _userProfileRepository.SaveChangesAsync();
            return true;
        }
    }
}
