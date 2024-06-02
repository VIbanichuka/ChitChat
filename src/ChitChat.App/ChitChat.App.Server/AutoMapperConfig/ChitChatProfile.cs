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
            CreateMap<UserProfilePhotoRequest, UserProfileDto>().ReverseMap();
            CreateMap<UserProfilePhotoResponse, UserProfileDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.UserProfile.ProfilePicture))
                .ReverseMap();
            CreateMap<UserRequestModel, UserDto>().ReverseMap();
            CreateMap<UserResponseModel, UserDto>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();

            CreateMap<Friendship, FriendshipDto>().ReverseMap();
            CreateMap<FriendshipDto, FriendshipResponseModel>().ReverseMap();

            CreateMap<Channel, ChannelDto>().ReverseMap();
            CreateMap<ChannelDto, ChannelResponseModel>().ReverseMap();
            CreateMap<ChannelDto, ChannelRequestModel>().ReverseMap();
        }
    }
}
