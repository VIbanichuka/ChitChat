using AutoMapper;
using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        [HttpGet]
        [ProducesResponseType(200)]
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

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(Guid userId) 
        { 
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                Log.Information("No user found.");
                return NotFound("User doesn't exist");
            }
            Log.Information("Returning user by ID: {UserId}", existingUser.UserId);
            var userResponse = _mapper.Map<UserResponseModel>(existingUser);

            return Ok(userResponse);
        }

        [HttpPost("Register")]
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

            var newUser = new User()
            {
                DisplayName = userRequest.DisplayName,
                Email = userRequest.Email,
            };

            Log.Information("User created successfully. User ID: {Email}", newUser.Email);
            await _userService.CreateUserAsync(newUser);

            var createdUser = _mapper.Map<UserResponseModel>(newUser);

            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, createdUser);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isDeleted = await _userService.DeleteUserAsync(userId);
            if (!isDeleted)
            {
                Log.Information("User not found to be deleted.");
                return NotFound("User not found");
            }
            Log.Information("User deleted successfully. User ID: {UserId}", userId);

            return NoContent();
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserRequestModel userRequest)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
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
