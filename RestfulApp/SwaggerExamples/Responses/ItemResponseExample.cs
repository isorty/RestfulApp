using RestfulApp.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.SwaggerExamples.Responses;

public class ItemResponseExample : IExamplesProvider<Response<ItemResponse>>
{
    public Response<ItemResponse> GetExamples() =>
        new()
        {
            Data = new()
            {
                Id = Guid.Empty,
                Name = "Created item",
                UserId = Guid.Empty.ToString()
            }
        };
}