using AutoMapper;
using ChitChat.AngularApp.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.AngularApp.Server.AutoMapperConfig
{
    public class ChitChatProfile: Profile
    {
        public ChitChatProfile()
        {
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserProfileRequest, UserProfileDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserModel, UserDto>().ReverseMap();
        }
    }
}
