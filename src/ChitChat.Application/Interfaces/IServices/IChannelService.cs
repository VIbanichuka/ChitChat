using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Interfaces.IServices
{
    public interface IChannelService
    {
        Task<IEnumerable<ChannelDto>> GetAllChannelsAsync();
        Task<ChannelDto> GetChannelByIdAsync(int id);
        Task<ChannelDto> GetChannelByNameAsync(string name);
        Task<ChannelDto> CreateChannelAsync(ChannelDto channel);
        Task<ChannelDto> UpdateChannelAsync(ChannelDto channel);
        Task<bool> DeleteChannelAsync(int id);
        Task<bool> LeaveChannel(int channelId, Guid userId);
        Task<bool> JoinChannel(int channelId, Guid userId);
        Task<bool> JoinChannelByDisplayName(int channelId, string displayName);
        Task<IEnumerable<MemberDto>> GetChannelMembersAsync(int channelId); 
        Task<IEnumerable<ChannelDto>> GetChannelsByUserIdAsync(Guid userId);
        Task<List<ChannelStatsDto>> GetTopChannelsByUsersAsync();
    }
}
