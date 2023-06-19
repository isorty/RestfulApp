using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Requests;

public sealed class RefreshTokenRequest : IRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}