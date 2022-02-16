using Microsoft.AspNetCore.Mvc;

namespace RestfulApp.Contracts.V1.Requests.Queries;

public class GetAllItemsQuery
{
    [FromQuery(Name = "name")]
    public string Name { get; set; }
}