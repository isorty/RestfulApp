using RestfulApp.Api.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.Api.SwaggerExamples.Requests;

public class CreateItemRequestExample : IExamplesProvider<CreateItemRequest>
{
    public CreateItemRequest GetExamples() =>
        new CreateItemRequest
        {
            Name = "New Item Name"
        };
}