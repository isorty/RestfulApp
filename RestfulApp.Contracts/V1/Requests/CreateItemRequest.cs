using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Requests;

public class CreateItemRequest : IRequest
{
    public string Name { get; set; }
}