using AutoMapper;
using RestfulApp.Data.Models;
using RestfulApp.Domain;

namespace RestfulApp.MappingProfiles;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Item, ItemDto>();
        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}