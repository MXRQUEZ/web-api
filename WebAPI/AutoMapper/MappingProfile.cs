using AutoMapper;
using Business.DTO;
using DAL.Model;

namespace WebAPI.AutoMapper
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserCredentialsDTO>()
                .ForMember(dest => dest.Email, source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.Email, source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.Password, source => source.MapFrom(source => source.PasswordHash));
            CreateMap<UserCredentialsDTO, User>()
                .ForMember(dest => dest.Email, source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.UserName, source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PasswordHash, source => source.MapFrom(source => source.Password));
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserName, source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.DateOfBirth, source => source.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.Gender, source => source.MapFrom(source => source.Gender))
                .ForMember(dest => dest.Email, source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.AddressDelivery, source => source.MapFrom(source => source.AddressDelivery));
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserName, source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.DateOfBirth, source => source.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.Gender, source => source.MapFrom(source => source.Gender))
                .ForMember(dest => dest.Email, source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.AddressDelivery, source => source.MapFrom(source => source.AddressDelivery));
        }
    }
}
