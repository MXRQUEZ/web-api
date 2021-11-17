using AutoMapper;
using Business.DTO;
using DAL.Models;

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

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Name, source => source.MapFrom(source => source.Name))
                .ForMember(dest => dest.Platform, source => source.MapFrom(source => source.Platform))
                .ForMember(dest => dest.DateCreated, source => source.MapFrom(source => source.DateCreated))
                .ForMember(dest => dest.TotalRating, source => source.MapFrom(source => source.TotalRating));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Name, source => source.MapFrom(source => source.Name))
                .ForMember(dest => dest.Platform, source => source.MapFrom(source => source.Platform))
                .ForMember(dest => dest.DateCreated, source => source.MapFrom(source => source.DateCreated))
                .ForMember(dest => dest.TotalRating, source => source.MapFrom(source => source.TotalRating));
        }
    }
}
