using RestfulApp.Api.Contracts.Interfaces;

namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class Response<TResponse> where TResponse : IResponse
{
    public TResponse Data { get; set; }

    public Response() { }

    public Response(TResponse response)
    {
        Data = response;
    }

    public static implicit operator Response<TResponse>(TResponse response) => new(response);
}