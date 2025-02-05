using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace ChitChat.Application.Implementations
{
    public class ExternalAuthService: IExternalAuthService
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public ExternalAuthService(IMapper mapper, IAuthService authService, IUserService userService, IConfiguration configuration)
        {
            _authService = authService;
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> AuthenticateWithGoogleAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(token));
            }

            var payload = await ValidateGoogleTokenAsync(token);
            var user = await GetOrRegisterUserAsync(payload);
            return _authService.CreateToken(user);
        }

        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string token)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            };

            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }

        private async Task<User> GetOrRegisterUserAsync(GoogleJsonWebSignature.Payload payload)
        {
            var user = await _userService.GetUserByEmailAsync(payload.Email);
            if (user != null)
            {
                return user;
            }

            var newUser = new UserDto
            {
                Email = payload.Email,
                DisplayName = payload.Email,
                UserProfile = new UserProfileDto
                {
                    ProfilePicture = payload.Picture,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
                }
            };
            var createdUser = await _userService.CreateUserAsync(_mapper.Map<User>(newUser));
            return _mapper.Map<User>(createdUser);
        }

    }
}
