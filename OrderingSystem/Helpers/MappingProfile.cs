using AutoMapper;
using OrderingSystem.API.Dtos;
using OrderingSystem.Core.Entities.Identity;
using OrderingSystem.Core.Entities.Order;

namespace OrderingSystem.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<Product, ProductToReturnDto>();


            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderItem, OrderItemToReturnDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.DisplayName));

            CreateMap<OrderItemDto, OrderItem>();

            CreateMap<OrderDto, Order>();

        }
    }
}