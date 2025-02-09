using System.Security.Claims;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IExternalAuthService _externalAuthService;

        public AuthController(IExternalAuthService externalAuthService, IPasswordService passwordService, IUserService userService, IAuthService authService)
        {
            _authService = authService;
            _passwordService = passwordService;
            _userService = userService;
            _externalAuthService = externalAuthService;
        }

        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel changePasswordRequest)
        {
            if(string.IsNullOrWhiteSpace(changePasswordRequest.CurrentPassword) || string.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
            {
                Log.Warning("Invalid user request. Current password and new password are needed.");
                return BadRequest("Invalid user request");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("Invalid user Id.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var isPasswordVerified = _passwordService.VerifyPasswordHash(changePasswordRequest.CurrentPassword, user.PasswordHash, user.PasswordSalt);
            
            if (!isPasswordVerified)
            {
                return Unauthorized("Incorrect password");
            }

            _passwordService.CreatePasswordHash(changePasswordRequest.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _userService.UpdateUserAsync(user);

            return Ok();
        }


        [HttpPost("Login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestModel userLoginRequest)
        {
            if (string.IsNullOrWhiteSpace(userLoginRequest.Email) || string.IsNullOrWhiteSpace(userLoginRequest.Password))
            {
                Log.Warning("Invalid user login request. Email or password is empty.");
                
                return BadRequest("Invalid user");
            }

            var user = await _userService.GetUserByEmailAsync(userLoginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }

            var isPasswordVerified = _passwordService.VerifyPasswordHash(userLoginRequest.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordVerified)
            {
                return Unauthorized("Incorrect password");
            }
            var token = _authService.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("google-login")]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthRequest request)
        {
            var token = await _externalAuthService.AuthenticateWithGoogleAsync(request.Token);

            return new JsonResult(new { token });
        }
    }
}
