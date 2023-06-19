using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class AuthFailedResponse : IResponse
{
    public IEnumerable<string> Errors { get; set; }
}