using AutoMapper;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Domain;

namespace RestfulApp.MappingProfiles;

public class DomainToResponseProfile : Profile
{
    public DomainToResponseProfile()
    {
        CreateMap<Item, ItemResponse>();
    }
}