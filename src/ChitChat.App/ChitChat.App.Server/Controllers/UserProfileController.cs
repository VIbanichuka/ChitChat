using AutoMapper;
using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;
        public UserProfileController(IUserProfileService userProfileService, IMapper mapper)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserProfileResponseModel>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserProfiles() 
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();

            if (userProfiles == null || !userProfiles.Any())
            {
                Log.Information("UserProfiles not found.");
                return NotFound();
            }         

            var userProfileResponse = _mapper.Map<IEnumerable<UserProfileResponseModel>>(userProfiles);

            return Ok(userProfileResponse);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserProfileResponseModel), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserProfileById(Guid id) 
        {
            if (id == Guid.Empty)
            {
                Log.Error("No userProfile Id");
                return BadRequest("Invalid user profile ID");
            }

            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);
            if (userProfile == null) 
            {
                Log.Information("UserProfile not found.");
                return NotFound();
            }
            
            var userProfileResponse = _mapper.Map<UserProfileResponseModel>(userProfile);

            return Ok(userProfileResponse);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileRequestModel userProfileRequest, [FromRoute] Guid id)
        {
            var existingUser = await _userProfileService.GetUserProfileByIdAsync(id);
            if (existingUser == null)
            {
                Log.Information("User profile not to be updated.");
                return NotFound("User Profile not found");
            }

            var updatedUserProfile = _mapper.Map(userProfileRequest, existingUser);

            await _userProfileService.UpdateUserProfileAsync(updatedUserProfile);

            var userProfileResponse = _mapper.Map<UserProfileResponseModel>(updatedUserProfile);

            return Ok(userProfileResponse);
        }
    }
}
