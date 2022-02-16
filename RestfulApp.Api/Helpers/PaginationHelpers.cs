using RestfulApp.Api.Domain;
using RestfulApp.Api.Services;
using RestfulApp.Contracts.Interfaces;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Api.Helpers;

public static class PaginationHelpers
{
    public static PaginatedResponse<TResponse> CreatePaginatedResponse<TResponse>(
        IUriService uriService, 
        PaginationFilter paginationFiler, 
        List<TResponse> responses) where TResponse : IResponse
    {
        var nextPage = paginationFiler.PageNumber >= 1 && responses.Count == paginationFiler.PageSize ?
            uriService.GetAllItemsUri(new PaginationFilter
            {
                PageNumber = paginationFiler.PageNumber + 1,
                PageSize = paginationFiler.PageSize
            }).ToString() : null;

        var previousPage = paginationFiler.PageNumber - 1 >= 1 ?
            uriService.GetAllItemsUri(new PaginationFilter
            {
                PageNumber = paginationFiler.PageNumber - 1,
                PageSize = paginationFiler.PageSize
            }).ToString() : null;

        return new PaginatedResponse<TResponse>()
        {
            Data = responses,
            PageNumber = paginationFiler.PageNumber >= 1 ? paginationFiler.PageNumber : null,
            PageSize = paginationFiler.PageSize >= 1 ? paginationFiler.PageSize : null,
            PreviousPage = previousPage,
            NextPage = nextPage
        };
    }
}
