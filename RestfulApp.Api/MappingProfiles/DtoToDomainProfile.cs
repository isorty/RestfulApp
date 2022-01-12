using AutoMapper;
using RestfulApp.Api.Data.Models;
using RestfulApp.Api.Domain;

namespace RestfulApp.Api.MappingProfiles;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile()
    {
        CreateMap<ItemDto, Item>()
            .ReverseMap()
            .ForMember(m => m.UserId, options => options.UseDestinationValue());

        CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
    }
}