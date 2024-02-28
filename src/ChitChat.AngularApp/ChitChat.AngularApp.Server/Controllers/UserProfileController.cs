using AutoMapper;
using ChitChat.AngularApp.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.Web.Controllers
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
        [ProducesResponseType(200)]
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

            return Ok(userProfiles);
        }
        
        [HttpGet("{userProfileId}")]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserProfileById(Guid userProfileId) 
        {
            if (userProfileId == Guid.Empty)
            {
                Log.Error("No userProfile Id");
                return BadRequest("Invalid user profile ID");
            }

            var userProfile = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (userProfile == null) 
            {
                Log.Information("UserProfile not found.");
                return NotFound();
            }
            
            return Ok(userProfile);
        }

        [HttpPut("{userProfileId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileRequest userProfileRequest, [FromRoute] Guid userProfileId)
        {
            var existingUser = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (existingUser == null)
            {
                Log.Information("User profile not to be updated.");
                return NotFound("User Profile not found");
            }

            var userProfile = _mapper.Map(userProfileRequest, existingUser);
            await _userProfileService.UpdateUserProfileAsync(userProfile);
            return NoContent();
        }
    }
}
