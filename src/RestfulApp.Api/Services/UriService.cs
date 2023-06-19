using Microsoft.AspNetCore.WebUtilities;
using RestfulApp.Api.Contracts.V1;
using RestfulApp.Api.Domain;
using RestfulApp.Api.Extensions;

namespace RestfulApp.Api.Services;

public class UriService : IUriService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UriService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Uri GetAllItemsUri(PaginationFilter paginationFilter = null)
    {
        var baseUri = GetBaseUri();

        if (paginationFilter is null)
        {
            return new Uri(baseUri);
        }

        var queryString = QueryHelpers.AddQueryString(baseUri, nameof(paginationFilter.PageNumber).ToLowerCamelCase(), paginationFilter.PageNumber.ToString());
        queryString = QueryHelpers.AddQueryString(queryString, nameof(paginationFilter.PageSize).ToLowerCamelCase(), paginationFilter.PageSize.ToString());

        return new Uri(queryString);
    }

    public Uri GetItemUri(string itemId) =>
        new Uri(GetBaseUri() +
            ApiRoutes.Items.Get.Replace(ApiRoutes.Items.ItemId, itemId));

    private string GetBaseUri()
    {
        var request = _contextAccessor.HttpContext.Request;

        return $"{request.Scheme}://{request.Host}{request.Path}";
    }        
}