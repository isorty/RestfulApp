using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Requests;

public class UserLoginRequest : IRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}