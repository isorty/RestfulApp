using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class ItemResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}