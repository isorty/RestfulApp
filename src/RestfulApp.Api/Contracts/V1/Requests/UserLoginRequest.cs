using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Requests;

public sealed class UserLoginRequest : IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}