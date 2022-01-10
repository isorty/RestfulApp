namespace RestfulApp.Contracts.V1.Requests.Queries;

public class PaginationQuery
{
    public const int MaxPageSize = 4000;

    public int PageNumber { get; set; }
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