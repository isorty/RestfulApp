using RestfulApp.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.SwaggerExamples.Responses;

public class ItemResponseExample : IExamplesProvider<ItemResponse>
{
    public ItemResponse GetExamples() =>
        new ItemResponse
        {
            Id = Guid.Empty,
            Name = "Created item",
            UserId = Guid.Empty.ToString()
        };
}