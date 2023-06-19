using Microsoft.AspNetCore.Mvc;

namespace RestfulApp.Api.Contracts.V1.Requests.Queries;

public sealed class GetAllItemsQuery
{
    [FromQuery(Name = "name")]
    public string Name { get; set; }
}