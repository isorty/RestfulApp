using AutoMapper;
using RestfulApp.Api.Data.Models;
using RestfulApp.Api.Domain;
using RestfulApp.Core.Objects;
using RestfulApp.Core.ValueObjects;

namespace RestfulApp.Api.MappingProfiles;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile()
    {
        CreateMap<ItemDto, Item>()
            .ForMember(dst => dst.Id, options => options.MapFrom(src => new ItemId(src.Id)))
            .ReverseMap()
            .ForMember(dst => dst.Id, options => options.MapFrom(src => src.Id.Value))
            .ForMember(dst => dst.UserId, options => options.UseDestinationValue());

        CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
    }
}