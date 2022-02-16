using RestfulApp.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.Api.SwaggerExamples.Responses;

public class PaginatedItemResponseExample : IExamplesProvider<PaginatedResponse<ItemResponse>>
{
    public PaginatedResponse<ItemResponse> GetExamples() =>
        new()
        {
            Data = new List<ItemResponse>()
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = "Sample name",
                    UserId = Guid.Empty.ToString()
                }
            },
            PageNumber = 0,
            PageSize = 0,
            NextPage = string.Empty,
            PreviousPage = string.Empty
        };
}