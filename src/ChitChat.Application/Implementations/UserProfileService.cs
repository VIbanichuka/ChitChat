﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;

namespace ChitChat.Application.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public UserProfileService(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);   
        }

        public async Task<UserProfileDto> GetUserProfileByIdAsync(Guid userProfileId)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(userProfileId);
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
    }
}
