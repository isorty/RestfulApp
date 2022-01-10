using AutoMapper;
using RestfulApp.Data.Models;
using RestfulApp.Domain;

namespace RestfulApp.MappingProfiles;

public class DtoToDomainProfile : Profile
{
    public DtoToDomainProfile()
    {
        CreateMap<ItemDto, Item>();
    }
}