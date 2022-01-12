using RestfulApp.Api.Domain;

namespace RestfulApp.Api.Services;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync(GetAllItemsFilter filter = null, PaginationFilter paginationFilter = null);
    Task<Item> GetItemByIdAsync(Guid itemId);
    Task<bool> CreateItemAsync(Item item);
    Task<Item> UpdateItemAsync(Item itemToUpdate);
    Task<bool> DeleteItemAsync(Guid itemId);
    Task<bool> UserOwnsItemAsync(Guid itemId, string userId);
}