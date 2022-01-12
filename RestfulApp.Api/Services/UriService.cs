using Microsoft.AspNetCore.WebUtilities;
using RestfulApp.Api.Domain;
using RestfulApp.Api.Extensions;
using RestfulApp.Contracts.V1;

namespace RestfulApp.Api.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetAllItemsUri(PaginationFilter paginationFilter = null)
    {
        if (paginationFilter is null)
        {
            return new Uri(_baseUri);
        }

        var queryString = QueryHelpers.AddQueryString(_baseUri, nameof(paginationFilter.PageNumber).ToLowerCamelCase(), paginationFilter.PageNumber.ToString());
        queryString = QueryHelpers.AddQueryString(queryString, nameof(paginationFilter.PageSize).ToLowerCamelCase(), paginationFilter.PageSize.ToString());

        return new Uri(queryString);
    }

    public Uri GetItemUri(string itemId) =>
        new Uri(_baseUri +
            ApiRoutes.Items.Get.Replace(ApiRoutes.Items.ItemId, itemId));
}