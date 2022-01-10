namespace RestfulApp.Contracts.V1.Responses;

public class Response<TResponse>
{
    public TResponse Data { get; set; }

    public Response() { }

    public Response(TResponse response)
    {
        Data = response;
    }
}