using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class LoginSuccessResponse : IResponse
{
    public string Token { get; set; }
}