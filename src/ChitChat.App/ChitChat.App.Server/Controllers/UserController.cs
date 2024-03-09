using AutoMapper;
using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public UserController(IUserService userService, IMapper mapper, IPasswordService passwordService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException();
            _passwordService = passwordService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponseModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers() 
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
            {
                Log.Information("No users found");
                return NotFound();
            }
            var mappedUsers = _mapper.Map<IEnumerable<UserResponseModel>>(users);
            return Ok(mappedUsers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponseModel), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(Guid id) 
        { 
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                Log.Information("No user found.");
                return NotFound("User doesn't exist");
            }
            Log.Information("Returning user by ID: {UserId}", existingUser.UserId);
            var userResponse = _mapper.Map<UserResponseModel>(existingUser);

            return Ok(userResponse);
        }

        [HttpGet("find-email/{email}")]
        [ProducesResponseType(typeof(UserResponseModel), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserByEmail(string email) 
        {
            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser == null)
            {
                return BadRequest("User does not exist");
            }
            else 
            {
                var userResponse = _mapper.Map<UserResponseModel>(existingUser);
                return Ok(userResponse);
            }
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckForUserEmail(string email)
        {
            var userExists = await _userService.CheckForEmail(email);
            if (userExists)
            {
                return BadRequest("Email exists");
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet("display-name/{displayName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CheckForDisplayName(string displayName)
        {
            var userExists = await _userService.CheckForDisplayName(displayName);
            if (userExists)
            {
                return BadRequest("Display name exists");
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser(UserRequestModel userRequest)
        {
            if (userRequest == null)
            {
                Log.Warning("Invalid user data provided.");
                return BadRequest("Invalid user data.");
            }
            _passwordService.CreatePasswordHash(userRequest.Password,out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User()
            {
                DisplayName = userRequest.DisplayName,
                Email = userRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            Log.Information("User created successfully. User ID: {Email}", newUser.Email);
            await _userService.CreateUserAsync(newUser);

            var createdUser = _mapper.Map<UserResponseModel>(newUser);

            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, createdUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                Log.Information("User not found to be deleted.");
                return NotFound("User not found");
            }
            Log.Information("User deleted successfully. User ID: {UserId}", id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequestModel userRequest)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                Log.Information("User not found to be updated.");
                return NotFound("User not found");
            }
            _mapper.Map(userRequest, existingUser);

            await _userService.UpdateUserAsync(existingUser);

            var updatedUser = _mapper.Map<UserResponseModel>(existingUser);

            return Ok(updatedUser);
        }
    }
}
