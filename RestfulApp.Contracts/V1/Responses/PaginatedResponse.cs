using RestfulApp.Contracts.Interfaces;

namespace RestfulApp.Contracts.V1.Responses;

public class PaginatedResponse<TResponse> where TResponse : IResponse
{
    public IEnumerable<TResponse> Data { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string NextPage { get; set; }
    public string PreviousPage { get; set; }

    public PaginatedResponse() { }

    public PaginatedResponse(IEnumerable<TResponse> responses)
    {
        Data = responses;
    }
}