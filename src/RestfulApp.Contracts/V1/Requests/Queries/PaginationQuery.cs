using Microsoft.AspNetCore.Mvc;

namespace RestfulApp.Contracts.V1.Requests.Queries;

public class PaginationQuery
{
    private const int MaxPageSize = 4000;

    [FromQuery(Name = "pageNumber")]
    public int PageNumber { get; set; }

    [FromQuery(Name = "pageSize")]
    public int PageSize { get; set; }

    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = MaxPageSize;
    }

    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize < MaxPageSize ? pageSize : MaxPageSize;
    }
}