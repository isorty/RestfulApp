using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Responses;

public class ItemResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
}