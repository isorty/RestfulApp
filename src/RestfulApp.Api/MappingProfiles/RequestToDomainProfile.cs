using AutoMapper;
using RestfulApp.Api.Contracts.V1.Requests;
using RestfulApp.Api.Contracts.V1.Requests.Queries;
using RestfulApp.Api.Domain;
using RestfulApp.Core.Objects;

namespace RestfulApp.Api.MappingProfiles;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<GetAllItemsQuery, GetAllItemsFilter>();
        CreateMap<PaginationQuery, PaginationFilter>();
        CreateMap<UpdateItemRequest, Item>();
        CreateMap<CreateItemRequest, Item>();
    }
}