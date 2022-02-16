using AutoMapper;
using RestfulApp.Api.Domain;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;

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