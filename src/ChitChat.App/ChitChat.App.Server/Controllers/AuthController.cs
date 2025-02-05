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
                return NotFound("Invalid Authentication");
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
