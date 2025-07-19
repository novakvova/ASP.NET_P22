using AutoMapper;
using Core.Models.Delivery;
using Core.Models.Order;
using Domain.Entities;
using Domain.Entities.Delivery;

namespace Core.Mapper;

public class OederMapper : Profile
{
    public OederMapper()
    {
        CreateMap<OrderItemEntity, OrderItemModel>()
            .ForMember(x => x.ProductImage, opt => opt
                .MapFrom(x => x.Product!.ProductImages!.OrderBy(x => x.Priority).First().Name))
            .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.Product!.Name))
            .ForMember(x => x.ProductSlug, opt => opt.MapFrom(x => x.Product!.Slug));

        CreateMap<OrderEntity, OrderModel>()
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.OrderStatus!.Name));

        CreateMap<CartEntity, OrderItemEntity>()
            .ForMember(x => x.PriceBuy, opt => opt
            .MapFrom(x => x.Product!.Price))
            .ForMember(x => x.Count, opt => opt
            .MapFrom(x => x.Quantity));
        
        CreateMap<DeliveryInfoCreateModel, DeliveryInfoEntity>();
    }
}
