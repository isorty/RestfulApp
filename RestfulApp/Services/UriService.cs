using Microsoft.AspNetCore.WebUtilities;
using RestfulApp.Contracts.V1;
using RestfulApp.Domain;

namespace RestfulApp.Services;

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

        var queryString = QueryHelpers.AddQueryString(_baseUri, nameof(paginationFilter.PageNumber), paginationFilter.PageNumber.ToString());
        queryString = QueryHelpers.AddQueryString(queryString, nameof(paginationFilter.PageSize), paginationFilter.PageSize.ToString());

        return new Uri(queryString);
    }

    public Uri GetItemUri(string itemId) => 
        new Uri(_baseUri + 
            ApiRoutes.Items.Get.Replace(ApiRoutes.Items.ItemId, itemId));
}