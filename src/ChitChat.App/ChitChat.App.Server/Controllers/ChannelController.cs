using System.Text.RegularExpressions;
using AutoMapper;
using ChitChat.App.Server.Hubs;
using ChitChat.App.Server.Hubs.Interfaces;
using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IHubContext<ChitChatHub, IChitChatHubClient> _hubContext;
        private readonly IChannelService _channelService;
        private readonly IMapper _mapper;
        public ChannelController(IChannelService channelService, IHubContext<ChitChatHub, IChitChatHubClient> hubContext, IMapper mapper)
        {
            _channelService = channelService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ChannelResponseModel), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetChannelById(int id) 
        {
            var existingChannel = await _channelService.GetChannelByIdAsync(id);
            if(existingChannel == null)
            {
                Log.Warning("No channel found");
                return NotFound();
            }
            var response = _mapper.Map<ChannelResponseModel>(existingChannel);
            return Ok(response);
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(typeof(ChannelResponseModel), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetChannelByName(string name)
        {
            var existingChannel = await _channelService.GetChannelByNameAsync(name);
            if (existingChannel == null)
            {
                Log.Warning("No channel found");
                return NotFound();
            }
            var response = _mapper.Map<ChannelResponseModel>(existingChannel);
            return Ok(response);
        }


        [HttpGet("user-channel/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ChannelResponseModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetChannelsByUserId(Guid userId)
        {
            var existingChannel = await _channelService.GetChannelsByUserIdAsync(userId);
            if (existingChannel == null)
            {
                Log.Warning("No channel found");
                return NotFound();
            }
            var response = _mapper.Map<IEnumerable<ChannelResponseModel>>(existingChannel);
            return Ok(response);
        }

        [HttpGet("members/{channelId}")]
        [ProducesResponseType(typeof(IEnumerable<ChannelResponseModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetChannelMembers(int channelId)
        {
            var members = await _channelService.GetChannelMembersAsync(channelId);
            if(!members.Any() || members == null)
            {
                Log.Information("No channel found");
                return NotFound();
            }
            return Ok(members);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChannelResponseModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllChannels()
        {
            var channels = await _channelService.GetAllChannelsAsync();
            if (!channels.Any() || channels == null)
            {
                Log.Information("No channel found");
                return NotFound();
            }
            var response = _mapper.Map<IEnumerable<ChannelResponseModel>>(channels);
            return Ok(response);
        }

        [HttpGet("channels")]
        [ProducesResponseType(typeof(IEnumerable<ChannelStatsDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTopChannels()
        {
            var channels = await _channelService.GetTopChannelsByUsersAsync();
            if (!channels.Any() || channels == null)
            {
                Log.Information("No channel found");
                return NotFound();
            }
            var response = _mapper.Map<IEnumerable<ChannelStatsDto>>(channels);
            return Ok(response);
        }

        [HttpPost("create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateChannel(ChannelRequestModel channelRequest)
        {
            if (channelRequest == null)
            {
                Log.Warning("Invalid data provided");
                return BadRequest();
            }
            var channelToCreate = new ChannelDto()
            {
                ChannelName = channelRequest.ChannelName,
            };
            await _channelService.CreateChannelAsync(channelToCreate);
            var createdChannel = _mapper.Map<ChannelResponseModel>(channelToCreate);
            return CreatedAtAction(nameof(GetChannelById), new { channelId = channelToCreate.ChannelId }, createdChannel);
        }

        [HttpPost("{channelId}/user/{userId}/join")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> JoinChannel(int channelId, Guid userId)
        {
            var hasJoined = await _channelService.JoinChannel(channelId, userId);
            if (hasJoined)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("{channelId}/displayName/{displayName}/join")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> JoinChannelByUserName(int channelId, string displayName)
        {
            var hasJoined = await _channelService.JoinChannelByDisplayName(channelId, displayName);
            if (hasJoined)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("{channelId}/user/{userId}/leave")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LeaveChannel(int channelId, Guid userId)
        {
            var hasLeft = await _channelService.LeaveChannel(channelId,userId);
            if (hasLeft)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{channelId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateChannel(int channelId, [FromBody] ChannelRequestModel channelRequest)
        {
            var existingChannel = await _channelService.GetChannelByIdAsync(channelId);
            if(existingChannel == null)
            {
                Log.Information("No channel found");
                return NotFound("Channel doesn't exist");
            }
            _mapper.Map(channelRequest, existingChannel);
            var channelToUpdate = await _channelService.UpdateChannelAsync(existingChannel);
            var updatedChannel = _mapper.Map<ChannelResponseModel>(channelToUpdate);
            return Ok(updatedChannel);
        }

        [HttpDelete("{channelId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteChannel(int channelId)
        {
            var isDeleted = await _channelService.DeleteChannelAsync(channelId);
            if (!isDeleted)
            {
                Log.Information("Channel not found to be deleted.");
                return NotFound("Channel not found");
            }
            Log.Information("Channel deleted successfully");

            return NoContent();
        }
    }
}
