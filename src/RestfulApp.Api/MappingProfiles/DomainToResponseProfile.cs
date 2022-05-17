using AutoMapper;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Core.Objects;

namespace RestfulApp.Api.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<Item, ItemResponse>()
            .ForMember(dst => dst.Id, options => options.MapFrom(src => src.Id.Value));
    }
}