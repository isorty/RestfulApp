using AutoMapper;
using RestfulApp.Api.Domain;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Api.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<Item, ItemResponse>()
            .ForMember(dst => dst.Id, options => options.MapFrom(src => src.Id.Value));
    }
}