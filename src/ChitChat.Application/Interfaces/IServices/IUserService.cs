using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(User user);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(Guid userId);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserDto> GetUserByIdAsync(Guid userId);
    }
}
