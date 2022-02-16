using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Requests;

public class UpdateItemRequest : IRequest
{
    public string Name { get; set; }
}