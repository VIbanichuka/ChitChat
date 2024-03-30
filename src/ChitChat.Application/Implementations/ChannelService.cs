using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Application.Interfaces.IRepositories;
using ChitChat.Application.Interfaces.IServices;
using ChitChat.Core.Entities;

namespace ChitChat.Application.Implementations
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ChannelService(IChannelRepository channelRepository, IMapper mapper, IUserRepository userRepository)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ChannelDto> CreateChannelAsync(ChannelDto channel)
        {
            if(string.IsNullOrWhiteSpace(channel.ChannelName)) 
            {
                throw new ArgumentNullException(nameof(channel), "Channel name cannot be null or empty.");
            }
            await CheckIfChannelExistsAsync(channel.ChannelName);
            var channelToCreate = _mapper.Map<Channel>(channel);
            await _channelRepository.AddAsync(channelToCreate);
            await _channelRepository.SaveChangesAsync();

            return _mapper.Map<ChannelDto>(channelToCreate);
        }

        private async Task<bool> CheckIfChannelExistsAsync(string name)
        {
            if (await _channelRepository.AnyAsync(c => c.ChannelName == name))
            {
                throw new ArgumentException("channel already exist");
            }
            return true;
        }

        public async Task<bool> DeleteChannelAsync(int id)
        {
            var existingChannel = await _channelRepository.FindAsync(c => c.ChannelId == id);
            if(existingChannel != null) 
            {
                _channelRepository.Remove(existingChannel);
                await _channelRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllChannelsAsync()
        {
            var channels = await _channelRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChannelDto>>(channels);
        }

        public async Task<ChannelDto> GetChannelByIdAsync(int id)
        {
            var channel = await _channelRepository.FindAsync(c => c.ChannelId == id);
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }
            return _mapper.Map<ChannelDto>(channel);
        }

        public async Task<ChannelDto> GetChannelByNameAsync(string name)
        {
            var channel = await _channelRepository.FindAsync(c => c.ChannelName == name);
            if (channel == null)
            {
                throw new ArgumentNullException($"{name} is not a channel");
            }
            return _mapper.Map<ChannelDto>(channel);
        }

        public async Task<bool> JoinChannel(int channelId, Guid userId)
        {
            var channel = await _channelRepository.GetChannelWithUserByIdAsync(channelId);
            if (channel == null)
            {
                throw new ArgumentNullException();
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                channel.Users.Add(user);
            }
            await _channelRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveChannel(int channelId, Guid userId)
        {
            var channel = await _channelRepository.GetChannelWithUserByIdAsync(channelId);
            if(channel == null) 
            { 
                throw new ArgumentNullException();
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) 
            { 
                throw new ArgumentNullException(nameof(user));
            }
            channel.Users.Remove(user);
            await _channelRepository.SaveChangesAsync();
            return true;
        }

        public async Task<ChannelDto> UpdateChannelAsync(ChannelDto channel)
        {
            var existingChannel = await _channelRepository.FindAsync(c => c.ChannelId == channel.ChannelId);
            if(existingChannel == null) 
            { 
                throw new ArgumentNullException(nameof(existingChannel));
            }
            var updatedChannel = _mapper.Map(channel, existingChannel);
            _channelRepository.Update(updatedChannel);
            await _channelRepository.SaveChangesAsync();

            return _mapper.Map<ChannelDto>(updatedChannel);
        }

        public async Task<IEnumerable<ChannelDto>> GetChannelsByUserIdAsync(Guid userId)
        {
            var channels = await _channelRepository.GetChannelsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ChannelDto>>(channels);
        }
    }
}
