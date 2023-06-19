using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Requests;

public sealed class UpdateItemRequest : IRequest
{
    public string Name { get; set; }
}