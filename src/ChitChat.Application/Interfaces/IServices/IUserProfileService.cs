using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync();
        Task<UserProfileDto> GetUserProfileByIdAsync(Guid userId);
        Task<UserProfileDto> UpdateUserProfileAsync(UserProfileDto userProfile);
    }
}
