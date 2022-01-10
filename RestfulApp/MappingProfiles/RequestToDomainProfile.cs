using AutoMapper;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Requests.Queries;
using RestfulApp.Domain;

namespace RestfulApp.MappingProfiles;

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