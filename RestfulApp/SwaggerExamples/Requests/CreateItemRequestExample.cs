﻿using RestfulApp.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace RestfulApp.SwaggerExamples.Requests;

public class CreateItemRequestExample : IExamplesProvider<CreateItemRequest>
{
    public CreateItemRequest GetExamples() =>
        new CreateItemRequest
        {
            Name = "New Item Name"
        };
}