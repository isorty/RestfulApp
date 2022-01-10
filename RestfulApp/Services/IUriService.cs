using RestfulApp.Domain;

namespace RestfulApp.Services;

public interface IUriService
{
    Uri GetItemUri(string itemId);
    Uri GetAllItemsUri(PaginationFilter paginationFilter = null);
}