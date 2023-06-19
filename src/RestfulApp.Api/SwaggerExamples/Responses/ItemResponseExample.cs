using RestfulApp.Api.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.Api.SwaggerExamples.Responses;

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