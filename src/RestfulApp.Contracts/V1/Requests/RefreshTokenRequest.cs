using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Requests;

public class RefreshTokenRequest : IRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}