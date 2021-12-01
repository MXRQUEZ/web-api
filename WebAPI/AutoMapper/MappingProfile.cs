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
                .ForMember(dest => dest.Email,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.Email,
                    source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.Password,
                    source => source.MapFrom(source => source.PasswordHash));
            CreateMap<UserCredentialsDTO, User>()
                .ForMember(dest => dest.Email,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.UserName,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PasswordHash,
                    source => source.MapFrom(source => source.Password));

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserName,
                    source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.DateOfBirth,
                    source => source.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.Gender,
                    source => source.MapFrom(source => source.Gender))
                .ForMember(dest => dest.Email,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber,
                    source => source.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.AddressDelivery,
                    source => source.MapFrom(source => source.AddressDelivery));
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserName,
                    source => source.MapFrom(source => source.UserName))
                .ForMember(dest => dest.DateOfBirth,
                    source => source.MapFrom(source => source.DateOfBirth))
                .ForMember(dest => dest.Gender,
                    source => source.MapFrom(source => source.Gender))
                .ForMember(dest => dest.Email,
                    source => source.MapFrom(source => source.Email))
                .ForMember(dest => dest.PhoneNumber,
                    source => source.MapFrom(source => source.PhoneNumber))
                .ForMember(dest => dest.AddressDelivery,
                    source => source.MapFrom(source => source.AddressDelivery));

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Name,
                    source => source.MapFrom(source => source.Name))
                .ForMember(dest => dest.Platform,
                    source => source.MapFrom(source => source.Platform))
                .ForMember(dest => dest.DateCreated,
                    source => source.MapFrom(source => source.DateCreated))
                .ForMember(dest => dest.Genre,
                    source => source.MapFrom(source => source.Genre))
                .ForMember(dest => dest.Rating,
                    source => source.MapFrom(source => source.Rating))
                .ForMember(dest => dest.Price,
                    source => source.MapFrom(source => source.Price))
                .ForMember(dest => dest.Count,
                    source => source.MapFrom(source => source.Count));
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.Name,
                    source => source.MapFrom(source => source.Name))
                .ForMember(dest => dest.Platform,
                    source => source.MapFrom(source => source.Platform))
                .ForMember(dest => dest.DateCreated,
                    source => source.MapFrom(source => source.DateCreated))
                .ForMember(dest => dest.Genre,
                    source => source.MapFrom(source => source.Genre))
                .ForMember(dest => dest.Rating,
                    source => source.MapFrom(source => source.Rating))
                .ForMember(dest => dest.Price,
                    source => source.MapFrom(source => source.Price))
                .ForMember(dest => dest.Count,
                    source => source.MapFrom(source => source.Count));

            CreateMap<Product, ProductOutputDTO>()
                .ForMember(dest => dest.TotalRating,
                    source => source.MapFrom(source => source.TotalRating))
                .ForMember(dest => dest.Logo,
                    source => source.MapFrom(source => source.Logo))
                .ForMember(dest => dest.Background,
                    source => source.MapFrom(source => source.Background));

            CreateMap<Order, OrderOutputDTO>()
                .ForMember(dest => dest.Id,
                    source => source.MapFrom(source => source.Id))
                .ForMember(dest => dest.CreationDate,
                    source => source.MapFrom(source => source.CreationDate))
                .ForMember(dest => dest.OrderItems,
                    source => source.MapFrom(source => source.OrderItems));

            CreateMap<OrderItemInputDTO, OrderItem>()
                .ForMember(dest => dest.ProductId,
                    source => source.MapFrom(source => source.ProductId))
                .ForMember(dest => dest.Amount,
                    source => source.MapFrom(source => source.Amount));
        }
    }
}
