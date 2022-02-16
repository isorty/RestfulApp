using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Responses;

public class LoginSuccessResponse : IResponse
{
    public string Token { get; set; }
}