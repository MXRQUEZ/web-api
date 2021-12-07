using AutoMapper;
using Business.DTO;
using DAL.Models.Entities;

namespace WebAPI.AutoMapper
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserCredentialsDTO>()
                .ForMember(dest => dest.Password,
                    source => source.MapFrom(source => source.PasswordHash));
            CreateMap<UserCredentialsDTO, User>()
                .ForMember(dest => dest.UserName,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PasswordHash,
                    source => source.MapFrom(source => source.Password));

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();

            CreateMap<Product, ProductOutputDTO>();

            CreateMap<Order, OrderOutputDTO>();

            CreateMap<OrderItemInputDTO, OrderItem>();
        }
    }
}
