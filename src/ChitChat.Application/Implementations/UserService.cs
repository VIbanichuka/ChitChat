using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.DisplayName))
            {
                throw new ArgumentNullException("Email and DisplayName are required fields");
            }

            await CheckIfUserExist(user.Email, user.DisplayName);
            var newUserProfile = new UserProfile();
            user.UserProfile = newUserProfile;
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        private async Task CheckIfUserExist(string email, string displayName)
        {

            if (await _userRepository.AnyAsync(u => u.Email == email || u.DisplayName == displayName))
            {
                throw new ArgumentException("User already exist");
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);
            if (existingUser != null) 
            { 
                _userRepository.Remove(existingUser);
                await _userRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return await _userRepository.FindAsync(user => user.Email == email);             
        }

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);

            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var updatedUser = _mapper.Map(user, existingUser);
            _userRepository.Update(updatedUser);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException();   
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
