using AutoMapper;
using RestfulApp.Data.Models;
using RestfulApp.Domain;

namespace RestfulApp.MappingProfiles;

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