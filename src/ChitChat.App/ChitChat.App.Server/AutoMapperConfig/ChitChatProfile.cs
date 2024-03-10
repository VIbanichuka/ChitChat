using AutoMapper;
using ChitChat.App.Server.Models.Reponses;
using ChitChat.App.Server.Models.Requests;
using ChitChat.Application.Dtos;
using ChitChat.Core.Entities;

namespace ChitChat.App.Server.AutoMapperConfig
{
    public class ChitChatProfile: Profile
    {
        public ChitChatProfile()
        {
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserProfileRequestModel, UserProfileDto>().ReverseMap();
            CreateMap<UserProfileResponseModel, UserProfileDto>().ReverseMap();
            CreateMap<UserProfileResponseModel, UserProfile>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRequestModel, UserDto>().ReverseMap();
            CreateMap<UserResponseModel, UserDto>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();

            CreateMap<Friendship, FriendshipDto>().ReverseMap();
        }
    }
}
