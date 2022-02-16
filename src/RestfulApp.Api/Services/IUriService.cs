using RestfulApp.Api.Domain;

namespace RestfulApp.Api.Services;

public interface IUriService
{
    Uri GetItemUri(string itemId);
    Uri GetAllItemsUri(PaginationFilter paginationFilter = null);
}