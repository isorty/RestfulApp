using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Responses;

public class AuthFailedResponse : IResponse
{
    public IEnumerable<string> Errors { get; set; }
}