using AutoMapper;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;
using ChitChat.Web.Models.Requests;

namespace ChitChat.Web.AutoMapperConfig
{
    public class ChitChatProfile: Profile
    {
        public ChitChatProfile()
        {
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserProfileRequest, UserProfileDto>().ReverseMap();
        }
    }
}
