using RestfulApp.Domain;

namespace RestfulApp.Services;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync();
    Task<Item> GetItemByIdAsync(Guid itemId);
    Task<bool> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(Item itemToUpdate);
    Task<bool> DeleteItemAsync(Guid itemId);
    Task<bool> UserOwnsItemAsync(Guid itemId, string userId);
}