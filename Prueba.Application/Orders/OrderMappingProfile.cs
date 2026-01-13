using AutoMapper;
using Prueba.Domain.Entities;

namespace Prueba.Application.Orders;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.OriginLat, opt => opt.MapFrom(s => s.Origin.Latitude))
            .ForMember(d => d.OriginLon, opt => opt.MapFrom(s => s.Origin.Longitude))
            .ForMember(d => d.DestinationLat, opt => opt.MapFrom(s => s.Destination.Latitude))
            .ForMember(d => d.DestinationLon, opt => opt.MapFrom(s => s.Destination.Longitude));
    }
}
