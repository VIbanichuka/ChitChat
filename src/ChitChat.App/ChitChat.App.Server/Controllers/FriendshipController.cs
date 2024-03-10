using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Implementations;
using ChitChat.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChitChat.App.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpGet("friends/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFriends(Guid userId)
        {
            var friends = await _friendshipService.GetAllFriendsAsync(userId);
            if (!friends.Any())
            {
                Log.Information("No users found");
                return NotFound();
            }
            return Ok(friends);
        }

        [HttpPost("send-request")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequestModel sendFriendRequestModel)
        {
            await _friendshipService.SendFriendRequestAsync(sendFriendRequestModel.InviterId,sendFriendRequestModel.InviteeId);
            return Ok(sendFriendRequestModel);
        }

        [HttpPut("accept-request/{friendshipId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AcceptFriendRequestAsync(int friendshipId) 
        {
            await _friendshipService.AcceptFriendRequestAsync(friendshipId);
            return Ok("Friend request accepted successfully.");
        }


        [HttpPut("reject-request/{friendshipId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RejectFriendRequestAsync(int friendshipId)
        {
            await _friendshipService.RejectFriendRequestAsync(friendshipId);
            return Ok("Friend request rejected successfully.");
        }

        [HttpGet("pending-requests/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPendingFriendRequests(Guid userId) 
        {
            var pendingRequests = await _friendshipService.GetPendingFriendRequestsAsync(userId);
            return Ok(pendingRequests);
        }
    }
}
